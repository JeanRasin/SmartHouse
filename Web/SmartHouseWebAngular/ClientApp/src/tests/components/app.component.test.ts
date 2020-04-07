import { NO_ERRORS_SCHEMA } from '@angular/core';
import { TestBed, async } from '@angular/core/testing';
import { AppComponent } from 'src/app';

export class AppComponentTest {
  static test() {
    describe('AppComponent', () => {
      beforeEach(async(() => {
        TestBed.configureTestingModule({
          declarations: [
            AppComponent
          ],
          schemas: [NO_ERRORS_SCHEMA],
        }).compileComponents();
      }));

      it('should create the app', () => {

        // Arrange
        const fixture = TestBed.createComponent(AppComponent);
        const app = fixture.debugElement.componentInstance;

        // Assert
        expect(app).toBeTruthy();
      });

    });
  }
}
