import { of } from 'rxjs';

export class MatDialogCreateMock {
  // When the component calls this.dialog.open(...) we'll return an object
  // with an afterClosed method that allows to subscribe to the dialog result observable.
  open() {
    return {
      afterClosed: () => of({ name: 'text', done: true })
    };
  }
}