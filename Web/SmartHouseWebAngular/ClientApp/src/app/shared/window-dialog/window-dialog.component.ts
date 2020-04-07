import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DialogTypeEnum } from '../enums';
import { DialogData } from '../models';

@Component({
  selector: 'app-window-dialog',
  templateUrl: 'window-dialog.component.html',
  styleUrls: ['window-dialog.component.scss']
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

  constructor(private dialogRef: MatDialogRef<WindowDialogComponent>, @Inject(MAT_DIALOG_DATA) data: DialogTypeEnum) {
    switch (data) {
      case DialogTypeEnum.Delete: this.data = this.masTexts[0]; break;
      case DialogTypeEnum.Uncheck: this.data = this.masTexts[1]; break;
    }
  }

  onOk() {
    this.dialogRef.close(true);
  }

  onExit() {
    this.dialogRef.close(false);
  }
}
