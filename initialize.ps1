#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Initializes the Hexalith.MyNewModule template with custom module and entity names.

.DESCRIPTION
    This script converts the Hexalith.MyNewModule template to a custom module.
    It replaces "MyNewModules" with the module name and "MyNewModule" with the entity name
    in all files and renames files/directories accordingly.

.PARAMETER ModuleName
    The name of the module (plural form). Example: "Customers", "GitStorages", "Orders"

.PARAMETER EntityName
    The name of the main entity/aggregate (singular form). Example: "Customer", "GitStorage", "Order"

.EXAMPLE
    ./initialize.ps1 -ModuleName "Customers" -EntityName "Customer"
    Creates a Customers module with Customer as the main aggregate.

.EXAMPLE
    ./initialize.ps1 -ModuleName "GitStorages" -EntityName "GitStorage"
    Creates a GitStorages module with GitStorage as the main aggregate.
#>
[CmdletBinding()]
param (
    [Parameter(Mandatory = $true, HelpMessage = "The module name (plural form, e.g., 'Customers')")]
    [string]$ModuleName,

    [Parameter(Mandatory = $true, HelpMessage = "The entity/aggregate name (singular form, e.g., 'Customer')")]
    [string]$EntityName
)

Write-Output "Initializing module: $ModuleName with entity: $EntityName"
Write-Verbose "Replacing 'MyNewModules' with '$ModuleName' in all files..."
Write-Verbose "Replacing 'MyNewModule' with '$EntityName' in all files..."
Write-Verbose "Also replacing lowercase variants..."

# Directories and files to exclude from processing (names only, no path separators)
$excludedDirectories = @(
    '.git',
    'Hexalith.Builds',
    'HexalithApp'
)
$excludedFiles = @(
    'initialize.ps1'
)

# Function to check if a path should be excluded
function ShouldExcludePath {
    param (
        [string]$Path
    )
    
    # Get the platform-specific directory separator
    $sep = [System.IO.Path]::DirectorySeparatorChar
    
    # Check excluded directories using cross-platform path patterns
    foreach ($excluded in $excludedDirectories) {
        # Build pattern that matches the directory name surrounded by separators
        # This handles paths like: /project/.git/file or D:\project\.git\file
        if ($Path -like "*$sep$excluded$sep*" -or $Path -like "*$sep$excluded") {
            return $true
        }
    }
    
    # Check excluded files
    $fileName = [System.IO.Path]::GetFileName($Path)
    if ($excludedFiles -contains $fileName) {
        return $true
    }
    
    return $false
}

# Function to process a file
function ProcessFile {
    param (
        [string]$FilePath
    )
    
    # Skip excluded paths and files
    if (ShouldExcludePath -Path $FilePath) {
        return
    }
    
    # Skip binary files
    $extension = [System.IO.Path]::GetExtension($FilePath)
    $binaryExtensions = @('.dll', '.exe', '.pdb', '.zip', '.obj', '.bin')
    
    if ($binaryExtensions -contains $extension) {
        return
    }
    
    try {
        $content = Get-Content -Path $FilePath -Raw -ErrorAction SilentlyContinue
        if ($null -eq $content) {
            return
        }
        
        $hasChanges = $false
        
        # IMPORTANT: Replace "MyNewModules" (plural) FIRST, then "MyNewModule" (singular)
        # This prevents partial replacements
        
        # Replace MyNewModules (plural) with ModuleName
        if ($content -match "MyNewModules") {
            $content = $content -replace "MyNewModules", $ModuleName
            $hasChanges = $true
        }
        
        # Replace mynewmodules (lowercase plural) with lowercase ModuleName
        if ($content -match "mynewmodules") {
            $content = $content -replace "mynewmodules", $ModuleName.ToLower()
            $hasChanges = $true
        }
        
        # Replace MyNewModule (singular) with EntityName
        if ($content -match "MyNewModule") {
            $content = $content -replace "MyNewModule", $EntityName
            $hasChanges = $true
        }
        
        # Replace mynewmodule (lowercase singular) with lowercase EntityName
        if ($content -match "mynewmodule") {
            $content = $content -replace "mynewmodule", $EntityName.ToLower()
            $hasChanges = $true
        }
        
        if ($hasChanges) {
            Set-Content -Path $FilePath -Value $content -NoNewline
            Write-Verbose "Updated: $FilePath"
        }
    }
    catch {
        Write-Warning "Could not process file: $FilePath"
        Write-Warning $_.Exception.Message
    }
}

# Function to rename files or directories matching a pattern
function Rename-ProjectItems {
    param (
        [Parameter(Mandatory = $true)]
        [string]$SearchPattern,
        
        [Parameter(Mandatory = $true)]
        [string]$Replacement,
        
        [Parameter(Mandatory = $true)]
        [ValidateSet('Directory', 'File')]
        [string]$ItemType
    )
    
    $getItemsParams = @{
        Path    = '.'
        Recurse = $true
    }
    if ($ItemType -eq 'Directory') {
        $getItemsParams.Directory = $true
    } else {
        $getItemsParams.File = $true
    }
    
    $items = Get-ChildItem @getItemsParams | Where-Object { 
        $_.Name -like "*$SearchPattern*" -and 
        -not (ShouldExcludePath -Path $_.FullName)
    }
    
    if ($ItemType -eq 'Directory') {
        # Sort directories by path depth (descending) to rename nested items first
        $items = $items | Sort-Object -Property FullName -Descending
    }
    
    foreach ($item in $items) {
        $newName = $item.Name -replace [regex]::Escape($SearchPattern), $Replacement
        $parentPath = if ($ItemType -eq 'Directory') { $item.Parent.FullName } else { $item.DirectoryName }
        $newPath = Join-Path -Path $parentPath -ChildPath $newName
        try {
            Rename-Item -Path $item.FullName -NewName $newName -Force -ErrorAction Stop
            Write-Verbose "Renamed $($ItemType): $($item.FullName) -> $newPath"
        }
        catch {
            Write-Warning "Could not rename $($ItemType): $($item.FullName)"
            Write-Warning $_.Exception.Message
        }
    }
}

# Get all files recursively from the current directory (excluding specified paths)
$files = Get-ChildItem -Path . -Recurse -File | Where-Object {
    -not (ShouldExcludePath -Path $_.FullName)
}

# Process each file content
foreach ($file in $files) {
    ProcessFile -FilePath $file.FullName
}

# IMPORTANT: Rename "MyNewModules" (plural) items FIRST, then "MyNewModule" (singular)
# This prevents partial renames

# Rename directories with MyNewModules (plural) to ModuleName
Rename-ProjectItems -SearchPattern "MyNewModules" -Replacement $ModuleName -ItemType Directory
Rename-ProjectItems -SearchPattern "MyNewModules" -Replacement $ModuleName -ItemType File

# Rename directories with mynewmodules (lowercase plural) to lowercase ModuleName
Rename-ProjectItems -SearchPattern "mynewmodules" -Replacement $ModuleName.ToLower() -ItemType Directory
Rename-ProjectItems -SearchPattern "mynewmodules" -Replacement $ModuleName.ToLower() -ItemType File

# Rename directories with MyNewModule (singular) to EntityName
Rename-ProjectItems -SearchPattern "MyNewModule" -Replacement $EntityName -ItemType Directory
Rename-ProjectItems -SearchPattern "MyNewModule" -Replacement $EntityName -ItemType File

# Rename directories with mynewmodule (lowercase singular) to lowercase EntityName
Rename-ProjectItems -SearchPattern "mynewmodule" -Replacement $EntityName.ToLower() -ItemType Directory
Rename-ProjectItems -SearchPattern "mynewmodule" -Replacement $EntityName.ToLower() -ItemType File

# Initialize and update Git submodules
Write-Output "`nInitializing Git submodules..."
try {
    # Check if .gitmodules file exists
    if (Test-Path ".gitmodules") {
        # Initialize submodules
        git submodule init
        Write-Verbose "Git submodules initialized"
        
        # Update submodules
        git submodule update
        Write-Verbose "Git submodules updated"
        
        # Checkout main branch for each submodule
        $submodules = git submodule foreach -q 'echo $name'
        foreach ($submodule in $submodules) {
            Write-Verbose "Checking out main branch for submodule: $submodule"
            Push-Location $submodule
            
            # Try to checkout main branch, if it fails try master
            git checkout main -q 2>$null
            if ($LASTEXITCODE -ne 0) {
                Write-Verbose "  Main branch not found, trying master..."
                git checkout master -q 2>$null
                if ($LASTEXITCODE -ne 0) {
                    Write-Warning "  Could not checkout main or master branch for $submodule"
                } else {
                    Write-Verbose "  Successfully checked out master branch for $submodule"
                }
            } else {
                Write-Verbose "  Successfully checked out main branch for $submodule"
            }
            
            Pop-Location
        }
    } else {
        Write-Verbose "No .gitmodules file found, skipping submodule initialization"
    }
}
catch {
    Write-Warning "Error during Git submodule operations"
    Write-Warning $_.Exception.Message
}

Write-Output "`nInitialization complete!"
Write-Output "Module name: 'MyNewModules' -> '$ModuleName'"
Write-Output "Entity name: 'MyNewModule' -> '$EntityName'"
Write-Output "`nYour new module structure:"
Write-Output "  - Hexalith.$ModuleName (module namespace)"
Write-Output "  - $EntityName (main aggregate/entity)"
