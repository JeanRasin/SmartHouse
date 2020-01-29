import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

import { Logger } from 'src/app/logger';
import { HttpService } from 'src/app/http.service';

@Component({
  selector: 'logger-component',
  templateUrl: './logger.component.html',
  styleUrls: ['logger.component.css'],
  providers: [HttpService]
})
export class LoggerComponent implements OnInit {
  error: any = null;

  constructor(private httpService: HttpService) { }

  displayedColumns: string[] = ['date', 'logLevel', 'eventId', 'message'];
  dataSource: MatTableDataSource<Logger>;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  pageSize: number = 10;

  ngOnInit() {
    this.httpService.getLogger().subscribe((data: Logger[]) => {

      data.sort(function (a, b) {
        return new Date(b.date).getTime() - new Date(a.date).getTime();
      });

      console.log(data);
      this.dataSource = new MatTableDataSource<Logger>(data);

      this.dataSource.paginator = this.paginator;
    }, error => {
      console.log(error);
      this.error = error;
    });
  }
}
