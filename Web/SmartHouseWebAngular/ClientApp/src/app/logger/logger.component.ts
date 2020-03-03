import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatTableDataSource } from '@angular/material';
import { Logger } from '../shared/models';
import { HttpLoggerService } from '../shared/services';

@Component({
  selector: 'app-logger-component',
  templateUrl: './logger.component.html',
  styleUrls: ['logger.component.scss'],
  providers: [HttpLoggerService]
})
export class LoggerComponent implements OnInit {
  pageSize = 10;
  error: any = null;
  displayedColumns: string[] = ['date', 'logLevel', 'eventId', 'message'];
  dataSource: MatTableDataSource<Logger>;

  constructor(private httpService: HttpLoggerService) { }

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  ngOnInit() {
    this.httpService.get().subscribe((data: Logger[]) => {

      data.sort(function (a, b) {
        return new Date(b.date).getTime() - new Date(a.date).getTime();
      });

      this.dataSource = new MatTableDataSource<Logger>(data);
      this.dataSource.paginator = this.paginator;
    }, error => {
      console.log(error);
      this.error = error;
    });
  }
}
