# ToolshedApp
A tool sharing application where users can borrow and loan tools amongst each other.
...
## Technologies
Asp.NET MVC
C#

## Acceptance Criteria

1. User will be able to add a tool with option of adding photo, category, and description.  
2. User will be able to edit and/or delete their own tools.
3. Only the owner of a tool will be able to 'return' a tool to available status in the system when a borrower brings it back to them.
4. Upon login, user will be able to view all tools that others have available for borrowing. 
5. User will have ability to view a list of of their own tools and the status of each. 
6. User will be able to track tools they are currently borrowing and tools they are currently loaning.
7. User will have a view that shows all loaned tools regardless of owner. 
8. User will be able to search for a tool by name, category, or description. 

## Advanced Features - not yet implemented 

1. Text notification: A borrower of a tool that has a prior reservation scheduled could receive text reminder to return the tool 24-hours prior to the reservation date.  
2. When user selects "now", it will reserve the tool immediately but must be switched to "checkout" within 4 hours otherwise tool will revert back to being available. 
3. User will be able to request a tool via text provided they themselves have signed up to recieve text requests from other users.
4.  After login, a user will see one of two screens:  
  1. If user has no tools available for loan, they will only have an option to "add a tool".  
  2. If a user has at least one tool listed on site for loan, they will have additional options for viewing tools by category, searching tools, or adding tools. This will allow the user to participate in borrowing tools.
5. Rating system: some type of system to provide feedback on the condition of, and/or the borrowing experience, relative to a specific tool/user. 
6. Suspend feature: means by which an owner may suspend a tool from being borrowed (perhaps because they are using it themselves).
7. User will be able to reserve, checkout, or return a tool.  
  1. When selecting reserve, user will be promted to select either "now" or a "future date".  
  2. Reserved.   