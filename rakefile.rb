# encoding: utf-8
require 'rubygems'
require 'albacore'
require 'rake/clean'

OUTPUT = "build"
CONFIGURATION = 'Release'
SOLUTION_FILE = 'src/Rest4Net.sln'

CLEAN.include(OUTPUT)
CLEAN.include(FileList["src/**/#{CONFIGURATION}"])

Albacore.configure do |config|
    config.log_level = :verbose
    config.msbuild.use :net4
end

desc "Compiles solution"
task :default => [:clean, :compile, :publish, :package]

desc "Compile solution file"
msbuild :compile do |msb|
    msb.properties :configuration => CONFIGURATION
    msb.targets :Clean, :Build
    msb.solution = SOLUTION_FILE
end

desc "Gathers output files and copies them to the output folder"
task :publish => [:compile] do
    Dir.mkdir(OUTPUT)
    Dir.mkdir("#{OUTPUT}/binaries")

    FileUtils.cp_r FileList["src/**/#{CONFIGURATION}/*.dll", "src/**/#{CONFIGURATION}/*.pdb"].exclude(/obj\//).exclude(/.Tests/), "#{OUTPUT}/binaries"
end

desc "Zips up the built binaries for easy distribution"
zip :package => [:publish] do |zip|
    Dir.mkdir("#{OUTPUT}/packages")

    zip.directories_to_zip "#{OUTPUT}/binaries"
    zip.output_file = "Rest4Net-Latest.zip"
    zip.output_path = "#{OUTPUT}/packages"
end

desc "Generates NuGet packages for each project that contains a nuspec"
task :nuget_package => [:publish] do
    Dir.mkdir("#{OUTPUT}/nuget")
    nuspecs = FileList["src/**/*.nuspec"]
    root = File.dirname(__FILE__)

    # Copy all project *.nuspec to nuget build folder before editing
    FileUtils.cp_r nuspecs, "#{OUTPUT}/nuget"
    nuspecs = FileList["#{OUTPUT}/nuget/*.nuspec"]

    # Update the copied *.nuspec files to correct version numbers and other common values
    nuspecs.each do |nuspec|
        update_xml nuspec do |xml|
            # Override common values
            xml.root.elements["metadata/authors"].text = "Oleksii Glib"
            xml.root.elements["metadata/licenseUrl"].text = "https://github.com/acropolium/Rest4Net/blob/master/license.txt"
            xml.root.elements["metadata/projectUrl"].text = "https://github.com/acropolium/Rest4Net"
        end
    end

    # Generate the NuGet packages from the newly edited nuspec fileiles
    nuspecs.each do |nuspec|        
        nuget = NuGetPack.new
        nuget.command = "tools/nuget/nuget.exe"
        nuget.nuspec = "\"" + root + '/' + nuspec + "\""
        nuget.output = "#{OUTPUT}/nuget"
        nuget.parameters = "-Symbols", "-BasePath \"#{root}\""     #using base_folder throws as there are two options that begin with b in nuget 1.4
        nuget.execute
    end
end

desc "Pushes the nuget packages in the nuget folder up to the nuget gallery and symbolsource.org. Also publishes the packages into the feeds."
task :nuget_publish do |task|
    nupkgs = FileList["#{OUTPUT}/nuget/*.nupkg"]
    nupkgs.each do |nupkg| 
        puts "Pushing #{nupkg}"
        nuget_push = NuGetPush.new
        nuget_push.package = File.expand_path(nupkg)
        nuget_push.create_only = false
        nuget_push.execute
    end
end

def update_xml(xml_path)
    #Open up the xml file
    xml_file = File.new(xml_path)
    xml = REXML::Document.new xml_file
 
    #Allow caller to make the changes
    yield xml
 
    xml_file.close
         
    #Save the changes
    xml_file = File.open(xml_path, "w")
    formatter = REXML::Formatters::Default.new(5)
    formatter.write(xml, xml_file)
    xml_file.close
end