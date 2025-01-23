# Chemical Data Processor in VBA

This Excel workbook allows users to load chemical data from a CSV file and calculate:
- Total cost of the chemicals.
- Percentage of parts that are natural.
- Whether the dataset is above 80% natural.

## How to Use
1. Open the `.xlsm` file in Excel.
2. Enable macros when prompted.
3. Use the following buttons:
   - **Load CSV**: Select and load a CSV file into the workbook.
   - **Run Command**: Process the data to calculate totals and display results.
4. View results on the first sheet.

## Installation
1.	In the VBA editor, go to Insert > Class Module.
2.	Rename the class module to Chemical.
3.	Add the class module Chemical code to Chemical:
4. Add the Main Script VBA code to the workbook (.xlsm)
5. Add buttons by: going to Developer > Insert > Button (Form Control).
6.	Place a button on the sheet and assign it to:
	•	Button 1: LoadCSV
	•	Button 2: RunCommand
