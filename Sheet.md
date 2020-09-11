# Sheets


## BottomSheets


### ExpandingBottomSheets


Controls Name: ExpandingBottomSheet
Control Style Name: MaterialExpandingBottomSheetStyle
Properties:
- IsExpanded: Boolean // Toggles Expanded and Collapsed States.
- IsMinimalSheet: Boolean // Toggles Collapsed and MinimalCollapsed States.
- CollapsedContent: Content 
- CollapsedContentTemplate: DataTemplate // Collapsed States Header Template.
- MinimalCollapsedContentTemplate: DataTemplate // MinimalCollapsed States Header Template.
- ExpandedCollapsedContent: Content 
- ExpandedContentTemplate: DataTemplate // Expanded States Content Template.
- ExpandedHeaderContentTemplate: DataTemplate // Expanded States Header Template.

Usage: Place Control in largest area of the current Page/Window to produce optimal behavior.

[see material.io documentation](https://material.io/components/sheets-bottom#expanding-bottom-sheet)

### StandardBottomSheets


Controls Name: StandardBottomSheet
Control Style Name: MaterialModalStandardBottomSheetStyle
Properties:
- Content: Content 
- ContentTemplate: DataTemplate
- HeaderContent: Content 
- HeaderContentTemplate: DataTemplate
- FullScreenHeaderContentTemplate: DataTemplate  // When sheet is FullScreen it will use this DataTemplate.
- SnapAreas: SheetSnapAreaCollection // Snap Points, used to stop flinging of Sheet.
- CurrentSnapArea: SheetSnapArea
- InitialSnapAreaName: String
- SheetPosition: Double // Value given to the corresponding Y position of the Sheet.

Usage: Place Control in largest area of the current Page/Window to produce expected behavior.

[see material.io documentation](https://material.io/components/sheets-bottom#standard-bottom-sheet)

### ModalBottomSheets


Controls Name: ModalBottomSheet
Control Style Name: MaterialModalStandardBottomSheetStyle
Properties:
- IsOpened: Boolean // Toggles Opened and Closed States.
- Content: Content 
- ContentTemplate: DataTemplate

Usage: Place Control in largest area of the current Page/Window, and bind IsOpened to a button.

[see material.io documentation](https://material.io/components/sheets-bottom#modal-bottom-sheet)

### ModalStandardBottomSheets


A combination between the standard and modal bottom sheets.

Controls Name: ModalStandardBottomSheet
Control Style Name: MaterialModalStandardBottomSheetStyle
Properties:
- IsOpened: Boolean // Toggles Opened and Closed States.
- Content: Content 
- ContentTemplate: DataTemplate
- HeaderContent: Content 
- HeaderContentTemplate: DataTemplate
- FullScreenHeaderContentTemplate: DataTemplate // When sheet is FullScreen it will use this DataTemplate.
- SnapAreas: SheetSnapAreaCollection // Snap Points, used to stop flinging of Sheet.
- CurrentSnapArea: SheetSnapArea
- InitialSnapAreaName: String
- SheetPosition: Double // Value given to the corresponding Y position of the Sheet.

Usage: Place Control in largest area of the current Page/Window, and bind IsOpened to a button.

[see material.io documentation](https://material.io/components/sheets-bottom)
