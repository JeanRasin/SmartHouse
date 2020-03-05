import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DialogData } from '../models';

@Component({
  selector: 'app-goal-dialog',
  templateUrl: './goal-create-dialog.component.html',
  styleUrls: ['./goal-create-dialog.component.scss']
})
export class GoalDialogCreateComponent implements OnInit {
  data: DialogData;
  name: string;
  done: boolean;
  form: FormGroup;

  constructor(
    fb: FormBuilder,
    private dialogRef: MatDialogRef<GoalDialogCreateComponent>,

    @Inject(MAT_DIALOG_DATA) data: any) {

    this.data = {
      title: 'Add goal',
      description: 'Enter goal description.'
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
