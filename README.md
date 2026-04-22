# Ticket Application System

## Project Information
- **Course**: Software design and testing C# (SDT621)
- **Module Code**: SDT621
- **NQF Level**: 6
- **Project Type**: Group Software Development Project (Week 2 Friday Project)
- **Minimum Group Members**: 4 students

## Project Description
A Windows Forms application that calculates ticket prices based on:
- Travel category (One/Two/Three)
- Distance traveled (in kilometers)
- Age-based discounts (free for under 12)
- Gender-based discounts (50% for female passengers)

## Features
- ✅ User-friendly Windows Forms interface
- ✅ Input validation for all fields
- ✅ Automatic price calculation
- ✅ Discount application (age and gender)
- ✅ Clear form functionality
- ✅ Exit confirmation dialog

## Pricing Rules

### Category Pricing
| Category | Price per km |
|----------|--------------|
| One      | R20          |
| Two      | R35          |
| Three    | R50          |

### Discount Rules
1. **Age < 12 years**: Ticket is **FREE** (R0.00)
2. **Female passengers**: **50% discount** applied

### Calculation Formula
```
Base Price = Category Rate × Distance
Final Price = Base Price (after age and gender discounts)
```

## How to Run the Application

### Option 1: Using Visual Studio
1. Open `TicketApplicationSystem.sln` in Visual Studio
2. Press `F5` or click the "Start" button
3. The application window will appear

### Option 2: Direct Execution
1. Navigate to `bin/Debug/` folder (after building)
2. Double-click `TicketApplicationSystem.exe`

## Project Structure
```
TicketApplicationSystem/
│
├── Form1.cs                      # Main form code
├── Form1.Designer.cs             # Form UI design (auto-generated)
├── Form1.resx                    # Form resources
├── TicketCalculator.cs           # Business logic class
├── Program.cs                    # Application entry point
├── App.config                    # Application configuration
├── TicketApplicationSystem.csproj # Project file
├── TicketApplicationSystem.sln   # Solution file
│
├── Properties/
│   ├── AssemblyInfo.cs          # Assembly metadata
│   ├── Resources.resx           # Application resources
│   ├── Resources.Designer.cs    # Resources code
│   ├── Settings.settings        # Application settings
│   └── Settings.Designer.cs     # Settings code
│
└── README.md                     # This file
```

## Group Member Roles

### Member 1: Project Manager / GitHub Coordinator
- Create GitHub repository
- Invite group members
- Create project structure
- Manage branches
- Merge final code
- Maintain README file
- Coordinate submission

### Member 2: UI Developer
- Design Windows Form interface
- Add all controls (labels, textboxes, radiobuttons, combobox, buttons)
- Apply professional styling
- Ensure responsive layout
- Properly name all controls

### Member 3: Logic Developer
- Write `TicketCalculator.cs` class
- Implement pricing logic
- Apply discount rules
- Wire up button events
- Implement validation logic

### Member 4: Tester and Documentation Specialist
- Test all functionality
- Verify calculations
- Document test cases
- Take screenshots with date/time
- Create bug reports
- Prepare final documentation

## GitHub Workflow

### Branch Structure
Each team member works on their own branch:
- `Dev-ui-design-[YourName]`
- `Dev-logic-module-[YourName]`
- `Dev-testing-docs-[YourName]`
- `Dev-project-management-[YourName]`

### Good Commit Message Examples
✅ "Created Windows Form layout"
✅ "Added gender selection controls"
✅ "Implemented category pricing logic"
✅ "Applied female discount rule"
✅ "Added validation for distance input"
✅ "Created test cases documentation"

❌ Avoid: "update", "changes", "fixed stuff"

## Testing Instructions

### Test Cases to Verify

| Test # | Name | Gender | Age | Category | Distance | Expected Price |
|--------|------|--------|-----|----------|----------|----------------|
| 1 | John | Male | 10 | One | 50 | R0.00 (FREE - under 12) |
| 2 | Sarah | Female | 25 | One | 100 | R1000.00 (50% discount) |
| 3 | Mike | Male | 30 | Two | 50 | R1750.00 |
| 4 | Lisa | Female | 10 | Three | 50 | R0.00 (FREE - under 12) |
| 5 | Tom | Male | 45 | Three | 100 | R5000.00 |
| 6 | Emma | Female | 35 | Two | 200 | R3500.00 (50% discount) |

### How to Test
1. Launch the application
2. Enter test data from the table above
3. Click "CALCULATE"
4. Verify the result matches expected price
5. Take a screenshot showing:
   - Full screen
   - Date and time in taskbar
   - Input values
   - Result message

## Input Validation

The application validates:
- ✅ Name is not empty
- ✅ Age is numeric (0-120)
- ✅ Distance is numeric and > 0
- ✅ Category is selected
- ✅ Gender is selected

## Deliverables Checklist

### For Submission
- [ ] GitHub Repository Link
- [ ] Working code with commits from all members
- [ ] Project Report (PDF) including:
  - [ ] Project title
  - [ ] Group members and student numbers
  - [ ] Member roles
  - [ ] Screenshots of application
  - [ ] Test results table
  - [ ] GitHub activity evidence
  - [ ] Challenges faced
  - [ ] Conclusion
- [ ] Individual Contribution Declaration (signed by each member)

## Technical Requirements

### Software Requirements
- Windows Operating System
- Visual Studio 2017 or later
- .NET Framework 4.7.2 or later

### Code Standards
- ✅ Proper naming conventions (camelCase for variables, PascalCase for methods)
- ✅ Comments explaining logic
- ✅ Meaningful variable names
- ✅ Clean, readable code structure

## Troubleshooting

### "Project won't build"
- Right-click solution → Restore NuGet Packages
- Clean Solution → Rebuild Solution

### "Form doesn't display correctly"
- Close and reopen the designer
- Rebuild the project

### "Controls not responding"
- Check that event handlers are properly wired in Designer

## Screenshots Required

Take screenshots showing:
1. GitHub repository with all members
2. Commit history from all members
3. Working application interface
4. Test cases with results
5. Successful calculations
6. Full screen with date/time visible

## License
Educational Project - CTU Training Solutions 2026

## Support
For questions about the project requirements, refer to:
- Week 2 Friday Project Documentation
- Your course instructor: Lusukama Selemani

---
**Hand Out Date**: 24 April 2026 @ 09:30 AM  
**Hand In Date**: 24 April 2026 @ 11:00 AM  
**Submission**: via GitHub link
