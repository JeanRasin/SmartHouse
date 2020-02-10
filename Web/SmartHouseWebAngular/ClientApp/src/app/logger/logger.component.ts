import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatTableDataSource } from '@angular/material';
import { Logger } from '../shared/models';
import { HttpLoggerService } from '../shared/services';

@Component({
  selector: 'logger-component',
  templateUrl: './logger.component.html',
  styleUrls: ['logger.component.css'],
  providers: [HttpLoggerService]
})
export class LoggerComponent implements OnInit {
  error: any = null;

  constructor(private httpService: HttpLoggerService) { }

  displayedColumns: string[] = ['date', 'logLevel', 'eventId', 'message'];
  dataSource: MatTableDataSource<Logger>;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  pageSize: number = 10;

  ngOnInit() {
    this.httpService.get().subscribe((data: Logger[]) => {

      data.sort(function (a, b) {
        return new Date(b.date).getTime() - new Date(a.date).getTime();
      });

      //console.log(data);
      this.dataSource = new MatTableDataSource<Logger>(data);
      this.dataSource.paginator = this.paginator;
    }, error => {
      console.log(error);
      this.error = error;
    });
  }
}
