import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DialogData } from '../models';

@Component({
  selector: 'app-goal-dialog',
  templateUrl: './goal-edit-dialog.component.html',
  styleUrls: ['./goal-edit-dialog.component.scss']
})
export class GoalDialogEditComponent implements OnInit {
  data: DialogData;
  name: string;
  done: boolean;
  form: FormGroup;

  constructor(
    fb: FormBuilder,
    private dialogRef: MatDialogRef<GoalDialogEditComponent>,
    @Inject(MAT_DIALOG_DATA) data: any) {
    
    this.data = {
      title: 'Edit goal',
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
