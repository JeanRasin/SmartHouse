import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { DialogData } from '../DialogData';
import { DialogType } from '../DialogTypeEnum';

@Component({//todo:@ ???
  selector: 'window-dialog',
  templateUrl: 'window-dialog.component.html',
  styleUrls: ['window-dialog.component.css']
})
export class WindowDialogComponent {

  masTexts: DialogData[] = [
    {
      title: 'Delete task',
      description: 'Are you sure you want to delete the task?'
    },
    {
      title: 'Cancel task',
      description: 'Are you sure you want to mark the task as not completed?'
    }
  ];
  data: DialogData;

  constructor(private dialogRef: MatDialogRef<WindowDialogComponent>, @Inject(MAT_DIALOG_DATA) data: DialogType) {
    switch (data) {
      case DialogType.Delete: this.data = this.masTexts[0]; break;
      case DialogType.Uncheck: this.data = this.masTexts[1]; break;
    }

  }

  onOk() {
    this.dialogRef.close(true);
  }

  onExit() {
    this.dialogRef.close(false);
  }
}
