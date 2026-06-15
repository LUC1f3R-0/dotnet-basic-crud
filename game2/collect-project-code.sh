#!/usr/bin/env bash

set -u

OUTPUT_FILE="game2-codebase-analysis.txt"

# Clear the previous output file.
: > "$OUTPUT_FILE"

write_section() {
    local title="$1"

    {
        echo
        echo "================================================================================"
        echo "$title"
        echo "================================================================================"
        echo
    } >> "$OUTPUT_FILE"
}

write_section "PROJECT LOCATION"

{
    echo "Current directory:"
    pwd

    echo
    echo ".NET SDK information:"
    dotnet --info 2>&1
} >> "$OUTPUT_FILE"

write_section "PROJECT DIRECTORY STRUCTURE"

find . \
    \( \
        -path './bin' -o \
        -path './bin/*' -o \
        -path './obj' -o \
        -path './obj/*' -o \
        -path './.git' -o \
        -path './.git/*' -o \
        -path './.vs' -o \
        -path './.vs/*' -o \
        -path './node_modules' -o \
        -path './node_modules/*' \
    \) -prune \
    -o -print \
    | sort \
    >> "$OUTPUT_FILE"

write_section "SOLUTION AND PROJECT FILES"

find . \
    -type f \
    \( \
        -name '*.sln' -o \
        -name '*.slnx' -o \
        -name '*.csproj' -o \
        -name '*.props' -o \
        -name '*.targets' \
    \) \
    ! -path './bin/*' \
    ! -path './obj/*' \
    ! -path './.git/*' \
    -print0 \
    | sort -z \
    | while IFS= read -r -d '' file; do
        {
            echo
            echo "--------------------------------------------------------------------------------"
            echo "FILE: $file"
            echo "--------------------------------------------------------------------------------"
            cat "$file"
            echo
        } >> "$OUTPUT_FILE"
    done

write_section "C# SOURCE FILES"

find . \
    -type f \
    -name '*.cs' \
    ! -path './bin/*' \
    ! -path './obj/*' \
    ! -path './.git/*' \
    -print0 \
    | sort -z \
    | while IFS= read -r -d '' file; do
        {
            echo
            echo "--------------------------------------------------------------------------------"
            echo "FILE: $file"
            echo "--------------------------------------------------------------------------------"

            # Show line numbers so individual errors can be referenced precisely.
            nl -ba "$file"

            echo
        } >> "$OUTPUT_FILE"
    done

write_section "CONFIGURATION FILE NAMES"

# File names are included, but their contents are intentionally excluded because
# appsettings and launch settings may contain passwords, tokens, or connection strings.
find . \
    -type f \
    \( \
        -name 'appsettings*.json' -o \
        -name 'launchSettings.json' \
    \) \
    ! -path './bin/*' \
    ! -path './obj/*' \
    ! -path './.git/*' \
    | sort \
    >> "$OUTPUT_FILE"

write_section "NAMESPACE DECLARATIONS"

grep -RIn \
    --include='*.cs' \
    --exclude-dir='bin' \
    --exclude-dir='obj' \
    --exclude-dir='.git' \
    -E '^[[:space:]]*namespace[[:space:]]+' \
    . 2>/dev/null \
    >> "$OUTPUT_FILE" || true

write_section "USING DIRECTIVES"

grep -RIn \
    --include='*.cs' \
    --exclude-dir='bin' \
    --exclude-dir='obj' \
    --exclude-dir='.git' \
    -E '^[[:space:]]*(global[[:space:]]+)?using[[:space:]]+' \
    . 2>/dev/null \
    >> "$OUTPUT_FILE" || true

write_section "CONTACT AND DBCONTEXT REFERENCES"

grep -RIn \
    --include='*.cs' \
    --exclude-dir='bin' \
    --exclude-dir='obj' \
    --exclude-dir='.git' \
    -E '\b(Contact|IContactRepository|ContactRepository|AppDbContext)\b' \
    . 2>/dev/null \
    >> "$OUTPUT_FILE" || true

write_section "DOTNET BUILD OUTPUT"

dotnet build --no-restore 2>&1 >> "$OUTPUT_FILE"

{
    echo
    echo "================================================================================"
    echo "END OF PROJECT SNAPSHOT"
    echo "================================================================================"
} >> "$OUTPUT_FILE"

echo
echo "Project snapshot created:"
echo "$(pwd)/$OUTPUT_FILE"
echo
echo "Review the file for passwords, API keys, tokens, and connection strings before sharing it."
