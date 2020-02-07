import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

import { MatDialog, MatDialogConfig, MatDialogRef, MatButtonToggleGroup } from '@angular/material';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faTrashAlt, faCheckCircle, faCircle, IconDefinition } from '@fortawesome/free-solid-svg-icons';

import { Goal } from 'src/app/goal';
import { HttpService } from 'src/app/http.service';
import { WindowDialogComponent } from '../window-dialog/window-dialog.component';
import { GoalDialogComponent } from '../goal-dialog/goal-dialog.component';
import { DialogType } from '../DialogTypeEnum';
import { OperationEnum } from '../OperationEnum';

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

  @ViewChild(MatButtonToggleGroup, { static: true }) group: MatButtonToggleGroup;
 // group.   this.group.value = 'all';
  //@ViewChildren(MatButtonToggle) toggles: QueryList<MatButtonToggle>;

  //toggleGroup: {
  //  value: 'all'
  //}

  faTrashAlt = faTrashAlt;
  faCheckCircle = faCheckCircle;
  faCircle = faCircle;

  //@ViewChild(MatPaginator, { static: true })
  paginator: MatPaginator;// = 'goals';
  pageSize: number = 10;

  ngOnInit() {
    this.group.value = 'goals';
    this.httpService.getGoal().subscribe((data: Goal[]) => {
      //console.log(data);
      this.dataSource = new MatTableDataSource<Goal>(data);
      this.dataSource.paginator = this.paginator;
    }, error => {
      console.log(error);
      // this.error = error;
    });
  }

  onValChange(value) {
    switch (value) {
      case 'goals':
        this.httpService.getGoal().subscribe((data: Goal[]) => {
          //console.log(data);
          this.dataSource = new MatTableDataSource<Goal>(data);
          this.dataSource.paginator = this.paginator;
        }, error => {
          console.log(error);
          // this.error = error;
        });
        break
      case 'all':
        this.httpService.getGoalAll().subscribe((data: Goal[]) => {
          //console.log(data);
          this.dataSource = new MatTableDataSource<Goal>(data);
          this.dataSource.paginator = this.paginator;
        }, error => {
          console.log(error);
          // this.error = error;
        });
        break
    }
  }

  onDelete(id: string) {
    let dialogConfig: MatDialogConfig = {
      width: '400px',
      height: '180px',
      data: DialogType.Delete
    };

    // dialogConfig.disableClose = true;
    // dialogConfig.autoFocus = true;

    const dialogRef = this.dialog.open(WindowDialogComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(
      data => {
        if (data == true) {
          //console.log('delete');
          this.delete(id);
        }
      });
  }

  onCheck(id: string, done: boolean) {
    const dialogConfig: MatDialogConfig = {
      width: '400px',
      height: '200px',
      data: DialogType.Uncheck
    };

    if (done == true) {
      const dialogRef = this.dialog.open(WindowDialogComponent, dialogConfig);
      dialogRef.afterClosed().subscribe(
        data => {
          if (data == true) {

            // console.log('check');
            this.check(id, false);
          }
        });
    }
    else {
      this.check(id, true);
    }
  }

  onCreate() {
    const dialogConfig: MatDialogConfig = {
      width: '450px',
      height: '260px',
      autoFocus: true,
     // disableClose :true,
      data: {
        type: OperationEnum.Create,
        name: null
      }
    };
    const dialogRef = this.dialog.open(GoalDialogComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(
      data => {
        if (data != null) {
          this.create(data.name);
          console.log('create');
        }
      });
  }

  onUpdate(id: string, done: boolean) {
    let dialogConfig: MatDialogConfig = {
      width: '450px',
      height: '260px',
      data: {
        type: OperationEnum.Update,
        name: null,
        done: null
      }
    };

    let data = this.dataSource.data.filter(d => d.id === id)[0];

    dialogConfig.data.name = data.name;
    dialogConfig.data.done = done;

    const dialogRef = this.dialog.open(GoalDialogComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(
      data => {
        if (data != null) {
          this.update(id, data.name, data.done);
          // console.log('update');
        }
      });
  }

  private delete(id: string) {
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

  private check(id: string, done: boolean) {
    this.httpService.checkGoal(id, done).subscribe(() => {

      //let data = this.dataSource.data.filter(d => d.id === id)[0];
      //data.done = done;
      //this.dataSource = new MatTableDataSource<Goal>(this.dataSource.data);
      //this.dataSource.paginator = this.paginator;

      this.setDataSource(id, undefined, done);


    }, error => {
      console.log(error);
      // this.error = error;
    });
  }

  private create(name: string) {
    this.httpService.createGoal(name).subscribe((data: Goal) => {

      this.dataSource.data.push(data);
      this.dataSource = new MatTableDataSource<Goal>(this.dataSource.data);
      this.dataSource.paginator = this.paginator;

    }, error => {
      console.log(error);
      // this.error = error;
    });
  }

  private update(id: string, name: string, done: boolean) {
    this.httpService.editGoal(name).subscribe(() => {

      //let data = this.dataSource.data.filter(d => d.id === id)[0];
      //data.name = name;
      //data.done = done;

      this.setDataSource(id, name, done);

      //this.dataSource = new MatTableDataSource<Goal>(this.dataSource.data);
      //this.dataSource.paginator = this.paginator;

    }, error => {
      console.log(error);
      // this.error = error;
    });
  }

  private setDataSource(id: string, name: string = undefined, done: boolean = undefined) {
    var item = this.dataSource.data.filter(d => d.id === id)[0];
    if (item != null) {

      if (name != undefined) {
        item.name = name;
      }

      if (done != undefined) {
        item.done = done;
      }

      this.dataSource = new MatTableDataSource<Goal>(this.dataSource.data);
      this.dataSource.paginator = this.paginator;
      // return item;
    }
    else {
      //error
    }
  }
}
