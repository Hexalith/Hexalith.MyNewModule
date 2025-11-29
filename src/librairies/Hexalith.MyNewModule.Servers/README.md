# Document Commands
This project contains the application commands for the Document application.

## Structure

### Document Commands
The Document commands handle operations related to document management:
- Document creation and modification
- Document metadata updates
- Document state changes
- File attachments and uploads

[Learn more about Document commands](./MyNewModule/README.md)

### Document Types Commands
The Document Types commands handle operations related to document type definitions:
- Document type creation and modification
- Field definitions and validations
- Document type metadata management
- Document type state workflows

[Learn more about Document Types commands](./DocumentTypes/README.md)

## Command Structure
Each command in this project follows the CQRS pattern and includes:
- Command properties for the operation parameters
- Validation rules
- Command handlers that process the business logic
- Integration with the domain events

## Usage
These commands are used through the command bus and can be invoked from:
- Web API endpoints
- UI components
- Background services
- Integration processes

The commands ensure consistency and maintain the business rules while performing operations on MyNewModule and document types.
