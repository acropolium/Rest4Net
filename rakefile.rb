# encoding: utf-8
require 'rubygems'
require 'rake/version_task'
Rake::VersionTask.new
require 'albacore'
require 'rake/clean'
require 'date'

OPTIONS_COMPANY = "Acropolium"
OPTIONS_AUTHOR = "Oleksii Glib"
OPTIONS_PROJECT = "Rest4Net"
OPTIONS_URL_PROJECT = "https://github.com/acropolium/Rest4Net"
OPTIONS_URL_LICENSE = "https://github.com/acropolium/Rest4Net/blob/master/license.txt"
OPTIONS_COPYRIGHT = "Copyright (c) "+OPTIONS_AUTHOR+" 2010-"+Date.today.strftime("%Y")

OUTPUT = "build"
SOLUTION_FILE = FileList["src/*.sln"][0]

CLEAN.include(OUTPUT)
CLEAN.include(FileList["src/**/bin"])
CLEAN.include(FileList["src/**/obj"])

Albacore.configure do |config|
	config.log_level = :non_verbose
    config.msbuild.use :net4
end

def win_dir
	ENV['windir'] || ENV['WINDIR'] || 'C:/Windows'
end

def get_net_version(netversion)
	case netversion
		when :net2, :net20, :net3, :net30
			version = 'v2.0.50727'
		when :net35
			version = 'v3.5'
		when :net4, :net40, :net45
			version = 'v4.0.30319'
		else
			fail_with_message "The .NET Framework #{netversion} is not supported"
	end
	File.join win_dir, 'Microsoft.NET', 'Framework', version
end

def get_configuration(netversion)
	case netversion
		when :net20
			version = '2.0'
		when :net35
			version = '3.5'
		when :net40
			version = '4.0'
		when :net45
			version = '4.5'
		else
			fail_with_message "The .NET Framework #{netversion} is not supported"
	end
	return 'Release ' + version
end

supportedNetVersions = [ :net20, :net35, :net40, :net45 ]
buildTasksNetVersions = supportedNetVersions.map { |ver| "build_#{ver}" }

task :compile => [:clean, :version] + buildTasksNetVersions

task :default => [:clean, :version, :compile, :publish, :package, :nuget_package, :nuget_publish]

supportedNetVersions.zip(buildTasksNetVersions) do |ver, task|
    msbuild task do |msb|
		msb.properties :configuration => (get_configuration ver)
		msb.targets :Clean, :Build
		msb.solution = SOLUTION_FILE
		msb.verbosity = 'quiet'
    end
end

desc "Generates AssemblyInfo files"
task :version do |asm|
    FileList["src/**/Properties/AssemblyInfo.*"].each { |assemblyfile|
        asm = AssemblyInfo.new()
        asm.use(assemblyfile)
        asm.version = Version.current
        asm.file_version = Version.current
        asm.company_name = OPTIONS_COMPANY
        asm.product_name = OPTIONS_PROJECT
        asm.title = File.basename(File.dirname(File.dirname(assemblyfile)))
        asm.copyright = OPTIONS_COPYRIGHT
        asm.execute
    }
end

desc "Gathers output files and copies them to the output folder"
task :publish => [:compile] do
    Dir.mkdir(OUTPUT)
    Dir.mkdir("#{OUTPUT}/binaries")

	supportedNetVersions.each do |ver|
		Dir.mkdir("#{OUTPUT}/binaries/#{ver}")
		FileUtils.cp_r FileList["src/**/#{ver}/*.dll", "src/**/#{ver}/*.pdb", "src/**/#{ver}/*.xml"].exclude(/obj\//).exclude(/.Tests/), "#{OUTPUT}/binaries/#{ver}"
	end
end

desc "Zips up the built binaries for easy distribution"
zip :package => [:publish] do |zip|
    Dir.mkdir("#{OUTPUT}/packages")

    zip.directories_to_zip "#{OUTPUT}/binaries"
    zip.output_file = "Rest4Net-Latest.zip"
    zip.output_path = "#{OUTPUT}/packages"
end

def xmlSetOrCreate(xml, node, key, value)
	el = node.elements[key]
	if !el
		el = node.add_element key
	end
	el.text = value
end

desc "Generates NuGet packages for each project that contains a nuspec"
task :nuget_package => [:publish] do
    Dir.mkdir("#{OUTPUT}/nuget")
    nuspecs = FileList["src/*/*.nuspec"].exclude(/packages\//)
    root = File.dirname(__FILE__)

    FileUtils.cp_r nuspecs, "#{OUTPUT}/nuget"
    nuspecs = FileList["#{OUTPUT}/nuget/*.nuspec"]

    nuspecs.each do |nuspec|
        update_xml nuspec do |xml|
			md = xml.root.elements["metadata"]
			xmlSetOrCreate(xml, md, "authors", OPTIONS_AUTHOR)
			xmlSetOrCreate(xml, md, "copyright", OPTIONS_COPYRIGHT)
			xmlSetOrCreate(xml, md, "owners", OPTIONS_AUTHOR)
			xmlSetOrCreate(xml, md, "version", Version.current)
			xmlSetOrCreate(xml, md, "licenseUrl", OPTIONS_URL_LICENSE)
			xmlSetOrCreate(xml, md, "projectUrl", OPTIONS_URL_PROJECT)
			xmlSetOrCreate(xml, md, "requireLicenseAcceptance", "false")
			
			md.elements.each("dependencies/dependency") do |e|
				if e.attributes["id"] == OPTIONS_PROJECT
					e.attributes["version"] = Version.current
				end
			end
			
			fe = xml.root.elements["files"]
			if !fe
				fe = REXML::Element.new('files')
				xml.root.insert_after('//metadata', fe)
			end
			ff = fe.add_element 'file'
			ff.add_attributes( {"src"=>("src/"+md.elements['id'].text+"/**/*.cs"), "target"=>"src"} )
			supportedNetVersions.each do |ver|
				ff = fe.add_element 'file'
				path = "#{OUTPUT}/binaries/#{ver}/"+md.elements['id'].text+".dll"
				ff.add_attributes( {"src"=>path, "target"=>"lib/#{ver}"} )
				path = "#{OUTPUT}/binaries/#{ver}/"+md.elements['id'].text+".xml"
				if File.file?(path)
					ff = fe.add_element 'file'
					ff.add_attributes( {"src"=>path, "target"=>"lib/#{ver}"} )
				end
			end
        end
    end

    nuspecs.each do |nuspec|
        nuget = NuGetPack.new
        nuget.command = "tools/nuget/nuget.exe"
        nuget.nuspec = "\"" + root + '/' + nuspec + "\""
        nuget.output = "#{OUTPUT}/nuget"
        nuget.parameters = "-Symbols", "-BasePath \"#{root}\""
        nuget.execute
    end
end

desc "Pushes the nuget packages in the nuget folder up to the nuget gallery and symbolsource.org. Also publishes the packages into the feeds."
task :nuget_publish => [:nuget_package] do |task|
	if Kernel.is_windows? == true
		separator = "\\\\"
	else
		separator = "/"
	end
    nupkgs = FileList["#{OUTPUT}/nuget/*.nupkg"]
    nupkgs.each do |nupkg|
		begin
			puts "Pushing #{nupkg}"
			nuget_push = NuGetPush.new
			nuget_push.command = "tools/nuget/nuget.exe"
			nuget_push.package = File.expand_path(nupkg).gsub("/", separator)
			nuget_push.create_only = false
			nuget_push.execute
		rescue
			puts "Skipping #{nupkg}"
		end
	end
end

def Kernel.is_windows?
	processor, platform, *rest = RUBY_PLATFORM.split("-")
	platform == 'mingw32'
end

def update_xml(xml_path)
    xml_file = File.new(xml_path)
    xml = REXML::Document.new xml_file
    yield xml
    xml_file.close
    xml_file = File.open(xml_path, "w")
    formatter = REXML::Formatters::Pretty.new()
    formatter.write(xml, xml_file)
    xml_file.close
end