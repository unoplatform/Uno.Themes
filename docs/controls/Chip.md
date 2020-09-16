# Chip

## Summary

`Chip` is a compact `ToggleButton` can be used for selection, filters or for a list of action to trigger. `ChipGroup` can be used to display a list of `Chip`. 

## Features

### Chip

| Properties         | Type         | Description                                                | Supported       |
|--------------------|--------------|------------------------------------------------------------|-----------------|
| IsCheckable        | bool         | Whether the chip can be checked. note: When used inside a ChipGroup, this property will be overwritten by ChipGroup's SelectionMode. | All platforms   |
| Thumbnail          | object       | Thumbnail to display on the chip.                          | All platforms   |
| ThumbnailTemplate  | DateTemplate | Template to display as the chip thumbanil.                 | All platforms   |
| CanRemove          | bool         | Whether there's a remove icon on the chip.                 | All platforms   |
| RemoveCommand      | TODO         | TODO                                                       | Not implemented |
| RemoveEvent        | TODO         | TODO                                                       | Not implemented |

### ChipGroup

| Properties         | Type              | Description                                                   | Supported       |
|--------------------|-------------------|---------------------------------------------------------------|-----------------|
| SelectionMode      | ChipSelectionMode | Gets or sets the selection behavior. (None, Single, Multiple) | All platforms   |
| SelectedItem       | object            | Current selected item. (SelectionMode = Single)               | All platforms   |
| SelectedItems      | IList             | Current selected items. (SelectionMode = Multiple)            | All platforms   |
| ThumbnailTemplate  | DataTemplate      | ThumbnailTemplate to use for each `Chip`.                     | All platforms   |
| CanRemove          | bool              | Whether we display a remove icon for each `Chip`              | All platforms   |
| RemoveCommand      | TODO              | TODO                                                          | Not implemented |
| RemoveEvent        | TODO              | TODO                                                          | Not implemented |

## Usage

### Chip

```xaml

<!-- Default material chip -->
<material:Chip Content="Chip"
            CanRemove="True"
			Style="{StaticResource MaterialChipStyle}"/>

<!-- Material chip with thumbnail-->
<material:Chip Content="Chip"
			   Style="{StaticResource MaterialChipStyle}">
	<material:Chip.Thumbnail>
		<!-- Thumbnail -->
	</material:Chip.Thumbnail>
</material:Chip>

```

### ChipGroup

```xaml

<!-- ChipGroup with static items -->
<material:ChipGroup Style="{StaticResource MaterialChipGroupStyle}">
    <material:Chip Content="Chip"
                   Style="{StaticResource MaterialChipStyle}" />
    <material:Chip Content="Chip"
                   IsChecked="True"
                   Style="{StaticResource MaterialChipStyle}" />
    <material:Chip Content="Chip"
                   Style="{StaticResource MaterialChipStyle}" />
</material:ChipBag>

<!-- ChipGroup with dynamic items -->
<material:ChipGroup ItemsSource="{Binding Items}"
                 Style="{StaticResource MaterialChipGroupStyle}"
                 SelectionMode="Single">

<!-- ChipGroup with custom thumbnail template -->
<material:ChipGroup ItemsSource="{Binding Items}"
                 Style="{StaticResource MaterialChipGroupStyle}"
                 ItemContainerStyle="{StaticResource MaterialOutlinedChipStyle}"
                 SelectionMode="Multiple">
                 
    <material:ChipGroup.ThumbnailTemplate>
        <DataTemplate>
             <!-- ThumbnailTemplate -->
        </DataTemplate>
    </material:ChipGroup.ThumbnailTemplate>
</material:ChipGroup>
```