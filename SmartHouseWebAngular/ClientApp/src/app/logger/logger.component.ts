import { Component, OnInit, ViewChild } from '@angular/core';
//import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

import { Logger } from 'src/app/logger';
import { HttpService } from 'src/app/http.service';

//export interface PeriodicElement {
//  name: string;
//  logLevel: number;
//  eventId: number;
//  message: string;
//}

//const ELEMENT_DATA: PeriodicElement[] = [
//  { position: 1, name: 'Hydrogen', weight: 1.0079, symbol: 'H' },
//  { position: 2, name: 'Helium', weight: 4.0026, symbol: 'He' },
//  { position: 3, name: 'Lithium', weight: 6.941, symbol: 'Li' },
//  { position: 4, name: 'Beryllium', weight: 9.0122, symbol: 'Be' },
//  { position: 5, name: 'Boron', weight: 10.811, symbol: 'B' },
//  { position: 6, name: 'Carbon', weight: 12.0107, symbol: 'C' },
//  { position: 7, name: 'Nitrogen', weight: 14.0067, symbol: 'N' },
//  { position: 8, name: 'Oxygen', weight: 15.9994, symbol: 'O' },
//  { position: 9, name: 'Fluorine', weight: 18.9984, symbol: 'F' },
//  { position: 10, name: 'Neon', weight: 20.1797, symbol: 'Ne' },
//];

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
  // data: Logger[];
  dataSource: MatTableDataSource<Logger>;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  pageSize = 10;

  ngOnInit() {
    this.httpService.getLogger().subscribe((data: Logger[]) => {

      data.sort(function (a, b) {
        // Turn your strings into dates, and then subtract them
        // to get a value that is either negative, positive, or zero.
        //return Math.abs(new Date(b.Date).getTime() - new Date(a.Date).getTime());
        return new Date(b.Date) - new Date(a.Date);
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
