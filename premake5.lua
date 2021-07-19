workspace "AptumEngine"
	
	--architecture "x64"

	configurations {
		"Debug",
		"Release",
		--"Distrib",
	}
	
	platforms {
		"Any CPU",
		"x64",
		"x86",
	}
	
	startproject "Sandbox"
	
	
outputdir = "%{cfg.buildcfg} - %{cfg.system}-%{cfg.architecture}"
	
	
project "Sandbox"
	location "Sandbox"
	kind "ConsoleApp"
	language "C#"
	-- clr "Unsafe"
	dotnetframework "netcoreapp2.1"
	
	targetdir ("_bin/" .. outputdir .. "/%{prj.name}")
	objdir ("_bin - obj/" .. outputdir .. "/%{prj.name}")
	
	files {
		"%{prj.name}/**.cs",
	}
	
	links {
		"AptumCore",
		"AptumGUI",
	}
	
	filter "configurations:Debug"
		defines "DEBUG"
		symbols "On"
	filter "configurations:Release"
		optimize "On"
	--filter "configurations:Distrib"
	--	optimize "On"
	
-- =========================== --
	
project "AptumCore"
	location "AptumCore"
	kind "SharedLib"
	language "C#"
	clr "Unsafe"
	-- buildoptions "/unsafe"
	dotnetframework  "netstandard2.1"
	
	targetdir ("_bin/" .. outputdir .. "/%{prj.name}")
	objdir ("_bin - obj/" .. outputdir .. "/%{prj.name}")
	
	files {
		"%{prj.name}/**.cs",
	}
	
	nuget {
		"Veldrid:4.8.0",
		"Veldrid.SDL2:4.8.0",
		"Veldrid.SPIRV:1.0.14",
		"Veldrid.StartupUtilities:4.8.0",
	}
	
	filter "configurations:Debug"
		defines "DEBUG"
		symbols "On"
	filter "configurations:Release"
		optimize "On"
	--filter "configurations:Distrib"
	--	optimize "On"
	
project "AptumGUI"
	location "AptumGUI"
	kind "SharedLib"
	language "C#"
	-- clr "Unsafe"
	dotnetframework  "netstandard2.1"
	--csversion "9.0"
	
	targetdir ("_bin/" .. outputdir .. "/%{prj.name}")
	objdir ("_bin - obj/" .. outputdir .. "/%{prj.name}")
	
	files {
		"%{prj.name}/**.cs",
	}
	
	links {
		"AptumCore",
	}
	
	filter "configurations:Debug"
		defines "DEBUG"
		symbols "On"
	filter "configurations:Release"
		optimize "On"
	--filter "configurations:Distrib"
	--	optimize "On"
		
		
		
		
		
		
		
		
		
		
		
		