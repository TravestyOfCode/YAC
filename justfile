# Use PowerShell instead of sh:
# set shell := ["powershell.exe", "-c"]
# set shell := ["windows-shell", ""]
set windows-shell := ["powershell.exe","-NoLogo", "-Command"]

project := "YAC"
outputDir := "Data/Migrations"

# Lists the available recipes.
default:
    just --list

# Adds a new migration to the project.
add-migration name:
	dotnet ef migrations add {{name}} --project {{project}} -o {{outputDir}}

# Removes the last migration from the project.
remove-migration:
    dotnet ef migrations remove --project {{project}}

# Updates the database to match latest migration.
update-database:
    dotnet ef database update --project {{project}}

# Adds package with specific version
add-package package *v:
    dotnet add {{project}} package {{package}} {{v}}

# Builds CSS using Tailwind CLI (Need to be in project directory for this to work)
css:
    tailwindcss -i .\wwwroot\css\site.css -o .\wwwroot\css\styles.css