import { Component, OnInit, ViewChild } from '@angular/core';
import { MatButtonToggleGroup, MatDialog, MatDialogConfig, MatSort, MatSortable } from '@angular/material';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { faCheckCircle, faCircle, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { Goal } from '../shared/models';
import { DialogTypeEnum, tableGoalSorting } from '../shared/enums';
import { GoalDialogCreateComponent, GoalDialogEditComponent } from '../shared/goal-helpers';
import { WindowDialogComponent } from '../shared/window-dialog';
import { HttpGoalService } from '../shared/services';

@Component({
  selector: 'app-goal-component',
  templateUrl: './goal.component.html',
  styleUrls: ['./goal.component.scss'],
  providers: [HttpGoalService]
})
export class GoalComponent implements OnInit {
  displayedColumns: string[] = ['check', 'name', 'dateCreate', 'remove'];

  private _dataSource: MatTableDataSource<Goal>;
  public get dataSource(): MatTableDataSource<Goal> {
    return this._dataSource;
  }
  public set dataSource(value: MatTableDataSource<Goal>) {
    this._dataSource = value;
  }

  faTrashAlt = faTrashAlt;
  faCheckCircle = faCheckCircle;
  faCircle = faCircle;

  pageSize = 5;

  constructor(private httpService: HttpGoalService, private dialog: MatDialog) { }

  @ViewChild(MatButtonToggleGroup, { static: true }) group: MatButtonToggleGroup;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;

  ngOnInit() {
    this.group.value = 'goals';
    this.httpService.get().subscribe((data: Goal[]) => {
      this.dataSource = new MatTableDataSource<Goal>(data);
      this.dataSource.paginator = this.paginator;

      this.sort.sort(({ id: 'dateUpdate', start: 'asc' }) as MatSortable);
      this.dataSource.sort = this.sort;
    }, error => {
      console.log(error);
    });
  }

  onValChange(value: tableGoalSorting) {
    switch (value) {
      case tableGoalSorting.goals:
        this.httpService.get().subscribe((data: Goal[]) => {
          this.dataSource = new MatTableDataSource<Goal>(data);
          this.dataSource.paginator = this.paginator;
        }, error => {
          console.log(error);
        });
        break;
      case tableGoalSorting.all:
        this.httpService.getAll().subscribe((data: Goal[]) => {
          this.dataSource = new MatTableDataSource<Goal>(data);
          this.dataSource.paginator = this.paginator;
        }, error => {
          console.log(error);
        });
        break;
    }
  }

  onDelete(id: string) {
    const dialogConfig: MatDialogConfig = {
      width: '400px',
      height: '180px',
      data: DialogTypeEnum.Delete
    };

    const dialogRef = this.dialog.open(WindowDialogComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(
      data => {
        if (data === true) {
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

    if (done === true) {
      const dialogRef = this.dialog.open(WindowDialogComponent, dialogConfig);
      dialogRef.afterClosed().subscribe(
        data => {
          if (data === true) {
            this.check(id, false);
          }
        });
    } else {
      this.check(id, true);
    }
  }

  onCreate() {
    const dialogConfig: MatDialogConfig = {
      width: '450px',
      height: '260px',
      autoFocus: true,
      data: {
        name: null
      }
    };

    const dialogRef = this.dialog.open(GoalDialogCreateComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(
      data => {
        if (data && data.name) {
          this.create(data.name);
        }
      });
  }

  onUpdate(id: string, done: boolean) {
    const dialogConfig: MatDialogConfig = {
      width: '450px',
      height: '280px',
      data: {
        name: null,
        done: null
      }
    };

    const data = this.dataSource.data.filter(d => d.id === id)[0];

    dialogConfig.data.name = data.name;
    dialogConfig.data.done = done;

    const dialogRef = this.dialog.open(GoalDialogEditComponent, dialogConfig);
    dialogRef
      .afterClosed()
      .subscribe(d => {
        if (d && d.name) {
          this.update(id, d.name, d.done);
        }
      });
  }

  private delete(id: string) {
    this.httpService.delete(id).subscribe(() => {
      const index = this.dataSource.data.findIndex(d => d.id === id);
      this.dataSource.data.splice(index, 1);
      this.dataSource.paginator = this.paginator;
    }, error => {
      console.log(error);
    });
  }

  private check(id: string, done: boolean) {
    this.httpService.check(id, done).subscribe(() => {
      this.setDataSource(id, undefined, done);
    }, error => {
      console.log(error);
    });
  }

  private create(name: string) {
    this.httpService.create(name).subscribe((data: Goal) => {
      this.dataSource.data.unshift(data);
      this.dataSource.paginator = this.paginator;
    }, error => {
      console.log(error);
    });
  }

  private update(id: string, name: string, done: boolean) {
    this.httpService.edit(id, name, done).subscribe(() => {
      this.setDataSource(id, name, done);
    }, (error: any) => {
      console.log(error);
    });
  }

  /**
   * Set data source.
   */
  private setDataSource(id: string, name: string = null, done: any = null) {
    const item = this.dataSource.data.filter(d => d.id === id)[0];
    if (item !== null) {

      if (name !== null) {
        item.name = name;
      }

      if (done !== null) {
        item.done = done;
      }

      this.dataSource.paginator = this.paginator;
    }
  }
}
