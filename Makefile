# Builds all the projects in the solution...
.PHONY: all_projects
all_projects: PetitsChevaux

# Builds project 'PetitsChevaux'...
.PHONY: PetitsChevaux
PetitsChevaux: 
	make --directory="WebSocketsTest/" --file=PetitsChevaux.makefile

# Cleans all projects...
.PHONY: clean
clean:
	make --directory="WebSocketsTest/" --file=PetitsChevaux.makefile clean

