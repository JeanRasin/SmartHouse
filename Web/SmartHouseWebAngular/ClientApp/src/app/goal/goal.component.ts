import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

import { MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faTrashAlt, faCheckCircle, faCircle, IconDefinition } from '@fortawesome/free-solid-svg-icons';

import { Goal } from 'src/app/goal';
import { HttpService } from 'src/app/http.service';
import { WindowDialogComponent } from '../window-dialog/window-dialog.component';

@Component({
  selector: 'goal-component',
  templateUrl: './goal.component.html',
  styleUrls: ['./goal.component.css'],
  providers: [HttpService]
})
export class GoalComponent implements OnInit {

  constructor(private httpService: HttpService, private dialog: MatDialog) { }

  displayedColumns: string[] = ['check', 'name', 'dateCreate', 'remove'];
  dataSource: MatTableDataSource<Goal>;

  faTrashAlt = faTrashAlt;
  faCheckCircle = faCheckCircle;
  faCircle = faCircle;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  pageSize: number = 10;

  ngOnInit() {
    this.httpService.getGoal().subscribe((data: Goal[]) => {
      //console.log(data);
      this.dataSource = new MatTableDataSource<Goal>(data);
      this.dataSource.paginator = this.paginator;
    }, error => {
      console.log(error);
      // this.error = error;
    });
  }

  delete(id: string) {
    let dialogConfig: MatDialogConfig = {
      width: '400px',
      height: '200px',
      data:  //{ type: DialogType.Delete }
    };

    // dialogConfig.disableClose = true;
    // dialogConfig.autoFocus = true;

    const dialogRef = this.dialog.open(WindowDialogComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(
      data => {
        if (data == true) {
          console.log('delete');
          this.deleteGoal(id);
        }
      });
  }

  check(id: string) {
    console.log('check');
    this.httpService.checkGoal(id).subscribe(() => {

      let data = this.dataSource.data.filter(d => d.id === id)[0];
      data.done = true;
      // this.dataSource.data.splice(index, 1);
      this.dataSource = new MatTableDataSource<Goal>(this.dataSource.data);

      this.dataSource.paginator = this.paginator;

    }, error => {
      console.log(error);
      // this.error = error;
    });
  }

  private deleteGoal(id: string) {
    this.httpService.deleteGoal(id).subscribe(() => {

      let index = this.dataSource.data.findIndex(d => d.id === id);
      this.dataSource.data.splice(index, 1);
      this.dataSource = new MatTableDataSource<Goal>(this.dataSource.data);

      //  this.refresh();

      this.dataSource.paginator = this.paginator;
    }, error => {
      console.log(error);
      // this.error = error;
    });
  }
}
