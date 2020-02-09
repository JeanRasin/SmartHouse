import { Component, OnInit, ViewChild } from '@angular/core';
import { MatButtonToggleGroup, MatDialog, MatDialogConfig } from '@angular/material';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { faCheckCircle, faCircle, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { HttpService } from 'src/app/http.service';
import { Goal } from '../shared/models';
import { OperationEnum, DialogTypeEnum } from '../shared/enums';
import { GoalDialogComponent } from '../shared/goal-helpers';
import { WindowDialogComponent } from '../shared/window-dialog';

@Component({
  selector: 'goal-component',
  templateUrl: './goal.component.html',
  styleUrls: ['./goal.component.scss'],
  providers: [HttpService]
})
export class GoalComponent implements OnInit {
  displayedColumns: string[] = ['check', 'name', 'dateCreate', 'remove'];
  dataSource: MatTableDataSource<Goal>;

  @ViewChild(MatButtonToggleGroup, { static: true }) group: MatButtonToggleGroup;
 
  faTrashAlt = faTrashAlt;
  faCheckCircle = faCheckCircle;
  faCircle = faCircle;

  paginator: MatPaginator;
  pageSize: number = 5;

  constructor(private httpService: HttpService, private dialog: MatDialog) { }

  ngOnInit() {
    this.group.value = 'goals';
    this.httpService.goal.get().subscribe((data: Goal[]) => {
      this.dataSource = new MatTableDataSource<Goal>(data);
      this.dataSource.paginator = this.paginator;
    }, error => {
      console.log(error);
    });
  }

  onValChange(value) {
    switch (value) {
      case 'goals':
        this.httpService.goal.get().subscribe((data: Goal[]) => {
          this.dataSource = new MatTableDataSource<Goal>(data);
          this.dataSource.paginator = this.paginator;
        }, error => {
          console.log(error);
          // this.error = error;
        });
        break
      case 'all':
        this.httpService.goal.getAll().subscribe((data: Goal[]) => {
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
      data: DialogTypeEnum.Delete
    };

    const dialogRef = this.dialog.open(WindowDialogComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(
      data => {
        if (data == true) {
          this.delete(id);
        }
      });
  }

  onCheck(id: string, done: boolean) {
    const dialogConfig: MatDialogConfig = {
      width: '400px',
      height: '200px',
      data: DialogTypeEnum.Uncheck
    };

    if (done == true) {
      const dialogRef = this.dialog.open(WindowDialogComponent, dialogConfig);
      dialogRef.afterClosed().subscribe(
        data => {
          if (data == true) {
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
        }
      });
  }

  private delete(id: string) {
    this.httpService.goal.delete(id).subscribe(() => {
      let index = this.dataSource.data.findIndex(d => d.id === id);
      this.dataSource.data.splice(index, 1);
      this.dataSource = new MatTableDataSource<Goal>(this.dataSource.data);
      this.dataSource.paginator = this.paginator;
    }, error => {
      console.log(error);
      // this.error = error;
    });
  }

  private check(id: string, done: boolean) {
    this.httpService.goal.check(id, done).subscribe(() => {
      this.setDataSource(id, undefined, done);
    }, error => {
      console.log(error);
      // this.error = error;
    });
  }

  private create(name: string) {
    this.httpService.goal.create(name).subscribe((data: Goal) => {

      this.dataSource.data.push(data);
      this.dataSource = new MatTableDataSource<Goal>(this.dataSource.data);
      this.dataSource.paginator = this.paginator;

    }, error => {
      console.log(error);
      // this.error = error;
    });
  }

  private update(id: string, name: string, done: boolean) {
    this.httpService.goal.edit(name).subscribe(() => {
      this.setDataSource(id, name, done);
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
    }
    else {
      //error
    }
  }
}
