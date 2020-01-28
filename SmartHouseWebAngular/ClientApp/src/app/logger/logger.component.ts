import { Component } from '@angular/core'

import { Logger } from 'src/app/logger'
import { HttpService } from 'src/app/http.service'

@Component({
  selector: 'logger-component',
  templateUrl: './logger.component.html',
  providers: [HttpService]
})
export class LoggerComponent {

  model: Logger[];
  error: any = null;

  constructor(private httpService: HttpService) { }

  ngOnInit() {
    this.httpService.getLogger().subscribe((data: Logger[]) => {
      console.log(data);
      this.model = data;
    }, error => {
      console.log(error);
      this.error = error;
    });
  }
}
