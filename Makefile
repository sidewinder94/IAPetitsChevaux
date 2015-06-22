# Builds all the projects in the solution...
.PHONY: all_projects
all_projects: PetitsChevaux TestsPetitsChevaux 

# Builds project 'PetitsChevaux'...
.PHONY: PetitsChevaux
PetitsChevaux: 
	nuget restore
	make --directory="WebSocketsTest/" --file=PetitsChevaux.makefile

# Builds project 'TestsPetitsChevaux'...
.PHONY: TestsPetitsChevaux
TestsPetitsChevaux: PetitsChevaux 
	make --directory="TestsPetitsChevaux/" --file=TestsPetitsChevaux.makefile

# Cleans all projects...
.PHONY: clean
clean:
	make --directory="WebSocketsTest/" --file=PetitsChevaux.makefile clean
	make --directory="TestsPetitsChevaux/" --file=TestsPetitsChevaux.makefile clean

