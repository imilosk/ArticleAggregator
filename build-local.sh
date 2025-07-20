#!/bin/bash

# Local build script that replicates the GitHub Actions workflow
# Usage: ./build-local.sh [OPTIONS]
#
# Options:
#   --scrape        Run data scraping after database migration
#   --migrate       Run database migrations
#   --test          Run .NET tests
#   --serve         Start local server after building (using pagefind --serve)
#   --help          Show this help message
#
# Default behavior: Build the complete site (CSS, JS, HTML, search index)

set -e  # Exit on any error

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Default options
SCRAPE=false
MIGRATE=false
SERVE=false
TEST=false
DOTNET_ENVIRONMENT=${DOTNET_ENVIRONMENT:-Development}

# Export the environment variable so all child processes can use it
export DOTNET_ENVIRONMENT

# Parse command line arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        --scrape)
            SCRAPE=true
            shift
            ;;
        --migrate)
            MIGRATE=true
            shift
            ;;
        --serve)
            SERVE=true
            shift
            ;;
        --test)
            TEST=true
            shift
            ;;
        --help)
            echo "Local build script that replicates the GitHub Actions workflow"
            echo ""
            echo "Usage: ./build-local.sh [OPTIONS]"
            echo ""
            echo "Options:"
            echo "  --scrape        Run data scraping after database migration"
            echo "  --migrate       Run database migrations only"
            echo "  --serve         Start local server after building (using pagefind --serve)"
            echo "  --help          Show this help message"
            echo ""
            echo "Default behavior: Build the complete site (CSS, JS, HTML, search index)"
            echo ""
            echo "Environment variables:"
            echo "  DOTNET_ENVIRONMENT  Set the .NET environment (default: Development)"
            exit 0
            ;;
        *)
            echo "Unknown option: $1"
            echo "Use --help for usage information"
            exit 1
            ;;
    esac
done

log() {
    echo -e "${BLUE}[$(date +'%Y-%m-%d %H:%M:%S')]${NC} $1"
}

error() {
    echo -e "${RED}[ERROR]${NC} $1" >&2
}

success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

if [[ ! -f "ArticleAggregator.sln" ]]; then
    error "This script must be run from the root project directory (where ArticleAggregator.sln is located)"
    exit 1
fi

check_dependencies() {
    log "Checking dependencies..."
    
    if ! command -v dotnet &> /dev/null; then
        error ".NET CLI is not installed or not in PATH"
        exit 1
    fi
    
    if ! command -v npm &> /dev/null; then
        error "npm is not installed or not in PATH"
        exit 1
    fi
    
    if ! command -v npx &> /dev/null; then
        error "npx is not installed or not in PATH"
        exit 1
    fi
    
    success "All dependencies are available"
}

restore_dotnet() {
    log "Restoring .NET dependencies..."
    dotnet restore
    success ".NET dependencies restored"
}

build_dotnet() {
    log "Building .NET projects..."
    dotnet build -c Release --no-restore
    success ".NET projects built"
}

test_dotnet() {
    log "Running .NET tests..."
    dotnet test --verbosity normal
    success ".NET tests completed"
}

migrate_database() {
    log "Running database migrations..."
    dotnet run --environment $DOTNET_ENVIRONMENT --project ArticleAggregator.DbMigrations
    success "Database migrations completed"
}

scrape_blogs() {
    log "Scraping blogs..."
    dotnet run --environment $DOTNET_ENVIRONMENT --project ArticleAggregator.DataIngest
    success "Blog scraping completed"
}

build_javascript() {
    log "Building JavaScript..."
    cd ArticleAggregator.BlogGenerator
    npm install
    npx mix --production
    cd ..
    success "JavaScript built"
}

generate_html() {
    log "Generating HTML..."
    dotnet run --environment $DOTNET_ENVIRONMENT --project ArticleAggregator.BlogGenerator
    success "HTML generated"
}

build_css() {
    log "Building CSS..."
    cd ArticleAggregator.BlogGenerator
    npm install
    npx tailwindcss -i ./input/css/styles.css -o ./output/css/styles.css --minify
    cd ..
    success "CSS built"
}

build_search_index() {
    log "Building search index..."
    cd ArticleAggregator.BlogGenerator
    npx -y pagefind --site output
    cd ..
    success "Search index built"
}

copy_assets() {
    log "Copying assets..."
    
    mkdir -p ./ArticleAggregator.BlogGenerator/output/js
    mkdir -p ./ArticleAggregator.BlogGenerator/output/images
    
    if [[ -d "./ArticleAggregator.BlogGenerator/mix/js/" ]]; then
        cp -r ./ArticleAggregator.BlogGenerator/mix/js/ ./ArticleAggregator.BlogGenerator/output/js
    else
        warning "Mix JS directory not found, skipping JS copy"
    fi
    
    if [[ -d "./ArticleAggregator.BlogGenerator/input/images/" ]]; then
        cp -r ./ArticleAggregator.BlogGenerator/input/images/ ./ArticleAggregator.BlogGenerator/output/images
    else
        warning "Images directory not found, skipping images copy"
    fi
    
    if [[ -f "./ArticleAggregator.BlogGenerator/input/favicon.ico" ]]; then
        cp ./ArticleAggregator.BlogGenerator/input/favicon.ico ./ArticleAggregator.BlogGenerator/output
    else
        warning "Favicon not found, skipping favicon copy"
    fi
    
    success "Assets copied"
}

serve_site() {
    log "Starting local server..."
    cd ArticleAggregator.BlogGenerator
    echo ""
    echo -e "${GREEN}🚀 Starting local server...${NC}"
    echo -e "${YELLOW}Press Ctrl+C to stop the server${NC}"
    echo ""
    npx -y pagefind --site output --serve
    cd ..
}

main() {
    log "Starting local build process..."
    log "Environment: $DOTNET_ENVIRONMENT"
    
    check_dependencies
    restore_dotnet
    build_dotnet
    
    if [[ "$MIGRATE" == true ]]; then
        migrate_database
    fi

    if [[ "$TEST" == true ]]; then
        test_dotnet
    fi

    if [[ "$SCRAPE" == true ]]; then
        scrape_blogs
    fi
    
    build_javascript
    generate_html
    build_css
    build_search_index
    copy_assets
    
    success "🎉 Build completed successfully!"
    
    if [[ "$SERVE" == true ]]; then
        serve_site
    else
        echo ""
        echo -e "${GREEN}✨ Your site is ready!${NC}"
        echo -e "${BLUE}📁 Output directory:${NC} ./ArticleAggregator.BlogGenerator/output"
        echo -e "${BLUE}🌐 To serve locally:${NC} ./build-local.sh --serve"
        echo ""
    fi
}

# Trap Ctrl+C and cleanup
trap 'echo -e "\n${YELLOW}Build interrupted by user${NC}"; exit 1' INT

main "$@"
