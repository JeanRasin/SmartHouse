import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-goal-dialog',
  templateUrl: './goal-edit-dialog.component.html',
  styleUrls: ['./goal-edit-dialog.component.scss']
})
export class GoalDialogEditComponent implements OnInit {
  name: string;
  done: boolean;
  form: FormGroup;

  constructor(
    fb: FormBuilder,
    private dialogRef: MatDialogRef<GoalDialogEditComponent>,
    @Inject(MAT_DIALOG_DATA) data: any) {
    this.name = data.name;
    this.done = data.done;
    this.form = fb.group({
      name: [this.name, Validators.required],
      done: [this.done]
    });
  }

  ngOnInit() {

  }

  onSave() {
    if (this.form.value.name) {
      this.dialogRef.close(this.form.value);
    }
  }

  onExit() {
    this.dialogRef.close(null);
  }
}
