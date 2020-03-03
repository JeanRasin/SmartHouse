import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { OperationEnum } from '../enums';
import { DialogData } from '../models';

@Component({
  selector: 'app-goal-dialog',
  templateUrl: './goal-dialog.component.html',
  styleUrls: ['./goal-dialog.component.scss']
})
export class GoalDialogComponent implements OnInit {

  masTexts: DialogData[] = [
    {
      title: 'Add goal',
      description: 'Enter goal description.'
    },
    {
      title: 'Edit goal',
      description: 'Enter goal description.'
    }
  ];
  data: DialogData;
  name: string;
  done: boolean;
  form: FormGroup;

  constructor(
    fb: FormBuilder,
    private dialogRef: MatDialogRef<GoalDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any) {
    switch (data.type) {
      case OperationEnum.Create: this.data = this.masTexts[0]; break;
      case OperationEnum.Update: this.data = this.masTexts[1]; break;
    }

    this.name = data.name;
    this.form = fb.group({
      name: [this.name, Validators.required],
    });
  }

  ngOnInit() {

  }

  onSave() {
    if (this.form.value.name != null) {
      this.dialogRef.close(this.form.value);
    }
  }

  onExit() {
    this.dialogRef.close(null);
  }
}
