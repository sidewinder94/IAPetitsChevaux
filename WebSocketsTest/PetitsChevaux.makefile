CSHARP_COMPILER = gmcs
FILES = Game/Board.cs Game/CaseType.cs Game/Pawn.cs Game/Player.cs Plans/MiniMax/MiniMax.cs Plans/MiniMax/NegaMax.cs Plans/MiniMax/Node.cs Plans/SimpleMinded.cs Program.cs Properties/AssemblyInfo.cs Properties/JetBrainsAnnotations.cs 
Debug_REFERENCES = -r:../packages/log4net.2.0.3/lib/net40-full/log4net.dll,../packages/Newtonsoft.Json.6.0.8/lib/net45/Newtonsoft.Json.dll,../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.Common.dll,../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.SocketBase.dll,../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.SocketEngine.dll,../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperWebSocket.dll
Release_REFERENCES = -r:../packages/log4net.2.0.3/lib/net40-full/log4net.dll,../packages/Newtonsoft.Json.6.0.8/lib/net45/Newtonsoft.Json.dll,../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.Common.dll,../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.SocketBase.dll,../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.SocketEngine.dll,../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperWebSocket.dll
Linux_REFERENCES = -r:../packages/log4net.2.0.3/lib/net40-full/log4net.dll,../packages/Newtonsoft.Json.6.0.8/lib/net45/Newtonsoft.Json.dll,../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.Common.dll,../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.SocketBase.dll,../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.SocketEngine.dll,../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperWebSocket.dll
Debug_FLAGS = -optimize- -define:DEBUG -define:TRACE -debug+ -debug:full -nowarn:1701,1702 -filealign:512 -warn:4 -pkg:dotnet 
Release_FLAGS = -optimize+ -define:TRACE -debug:pdbonly -nowarn:1701,1702 -filealign:512 -warn:4 -pkg:dotnet 
Linux_FLAGS = -optimize+ -define:TRACE -debug:pdbonly -nowarn:1701,1702 -filealign:512 -warn:4 -pkg:dotnet 
OUTPUT_FILE = PetitsChevaux.exe
Debug_OUTPUT_FOLDER = bin/monoDebug
Release_OUTPUT_FOLDER = bin/monoRelease
Linux_OUTPUT_FOLDER = bin/monoLinux
TARGET = exe

# Builds all configurations for this project...
.PHONY: build_all_configurations
build_all_configurations: Debug Release Linux 

# Builds the Debug configuration...
.PHONY: Debug
Debug: create_folders $(FILES)
	$(CSHARP_COMPILER) $(Debug_REFERENCES) $(Debug_FLAGS) -out:$(Debug_OUTPUT_FOLDER)/$(OUTPUT_FILE) -target:$(TARGET) $(FILES)

# Builds the Release configuration...
.PHONY: Release
Release: create_folders $(FILES)
	$(CSHARP_COMPILER) $(Release_REFERENCES) $(Release_FLAGS) -out:$(Release_OUTPUT_FOLDER)/$(OUTPUT_FILE) -target:$(TARGET) $(FILES)

# Builds the Linux configuration...
.PHONY: Linux
Linux: create_folders $(FILES)
	$(CSHARP_COMPILER) $(Linux_REFERENCES) $(Linux_FLAGS) -out:$(Linux_OUTPUT_FOLDER)/$(OUTPUT_FILE) -target:$(TARGET) $(FILES)

# Creates the output folders for each configuration, and copies references...
.PHONY: create_folders
create_folders:
	mkdir -p $(Debug_OUTPUT_FOLDER)
	cp ../packages/log4net.2.0.3/lib/net40-full/log4net.dll $(Debug_OUTPUT_FOLDER)
	cp ../packages/Newtonsoft.Json.6.0.8/lib/net45/Newtonsoft.Json.dll $(Debug_OUTPUT_FOLDER)
	cp ../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.Common.dll $(Debug_OUTPUT_FOLDER)
	cp ../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.SocketBase.dll $(Debug_OUTPUT_FOLDER)
	cp ../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.SocketEngine.dll $(Debug_OUTPUT_FOLDER)
	cp ../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperWebSocket.dll $(Debug_OUTPUT_FOLDER)
	mkdir -p $(Release_OUTPUT_FOLDER)
	cp ../packages/log4net.2.0.3/lib/net40-full/log4net.dll $(Release_OUTPUT_FOLDER)
	cp ../packages/Newtonsoft.Json.6.0.8/lib/net45/Newtonsoft.Json.dll $(Release_OUTPUT_FOLDER)
	cp ../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.Common.dll $(Release_OUTPUT_FOLDER)
	cp ../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.SocketBase.dll $(Release_OUTPUT_FOLDER)
	cp ../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.SocketEngine.dll $(Release_OUTPUT_FOLDER)
	cp ../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperWebSocket.dll $(Release_OUTPUT_FOLDER)
	mkdir -p $(Linux_OUTPUT_FOLDER)
	cp ../packages/log4net.2.0.3/lib/net40-full/log4net.dll $(Linux_OUTPUT_FOLDER)
	cp ../packages/Newtonsoft.Json.6.0.8/lib/net45/Newtonsoft.Json.dll $(Linux_OUTPUT_FOLDER)
	cp ../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.Common.dll $(Linux_OUTPUT_FOLDER)
	cp ../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.SocketBase.dll $(Linux_OUTPUT_FOLDER)
	cp ../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperSocket.SocketEngine.dll $(Linux_OUTPUT_FOLDER)
	cp ../packages/SuperWebSocket.0.9.0.2/lib/net40/SuperWebSocket.dll $(Linux_OUTPUT_FOLDER)

# Cleans output files...
.PHONY: clean
clean:
	rm -f $(Debug_OUTPUT_FOLDER)/*.*
	rm -f $(Release_OUTPUT_FOLDER)/*.*
	rm -f $(Linux_OUTPUT_FOLDER)/*.*

