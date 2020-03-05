import { NO_ERRORS_SCHEMA } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { Goal, SharedModule, HttpGoalService, tableGoalSorting } from 'src/app/shared';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { MatTableModule, MatPaginatorModule, MatButtonToggleModule, MatButtonModule, MatDialogModule, MatDialog, MatTableDataSource } from '@angular/material';
import { GoalComponent } from 'src/app/goal/goal.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { MatDialogMock } from '../mocks/mat-dialog-mock';
import { MatDialogCreateMock } from '../mocks/mat-dialog-create-mock';
import { analyzeAndValidateNgModules } from '@angular/compiler';

export class GoalComponentTest {
    static test() {

        // todo: сделать проверку на то если нет связи с апи.
        describe('Goal component', () => {

            // #region region  test data
            const testData: Goal[] = [
                {
                    id: 'c55940df-c000-c450-1601-5e9aa3ed8bbc',
                    name: 'driver Officer Holy See (Vatican City State)',
                    dateCreate: new Date('1997-01-13T02:08:14.756456'),
                    dateUpdate: new Date('1997-01-29T18:59:41.684245'),
                    done: false
                },
                {
                    id: '38874b7d-ef81-4895-b86f-f718c4a81544',
                    name: 'Plastic morph Balanced',
                    dateCreate: new Date('1997-01-15T21:54:29.920099'),
                    dateUpdate: new Date('1997-01-28T22:29:44.035771'),
                    done: false
                },
                {
                    id: '7dc7631d-3f1e-f8bb-166c-63b52a05db21',
                    name: 'Ports Rupiah Awesome',
                    dateCreate: new Date('1997-01-13T23:07:22.848384'),
                    dateUpdate: new Date('1997-01-27T16:35:23.823537'),
                    done: false
                },
                {
                    id: '0037c991-eb6d-2819-491c-221a1c9c01a8',
                    name: 'matrix Stravenue Usability',
                    dateCreate: new Date('1997-01-06T11:03:38.286677'),
                    dateUpdate: new Date('1997-01-26T17:58:46.010328'),
                    done: false
                },
                {
                    id: '735f337f-755a-4db6-19ef-a092f0c82c8c',
                    name: 'Trail Handcrafted Granite Chicken Shores',
                    dateCreate: new Date('1997-01-16T00:30:24.257496'),
                    dateUpdate: new Date('1997-01-26T10:41:03.225911'),
                    done: false
                }];

            const testDataAll: Goal[] = [
                {
                    id: 'c55940df-c000-c450-1601-5e9aa3ed8bbc',
                    name: 'driver Officer Holy See (Vatican City State)',
                    dateCreate: new Date('1997-01-13T02:08:14.756456'),
                    dateUpdate: new Date('1997-01-29T18:59:41.684245'),
                    done: false
                },
                {
                    id: '38874b7d-ef81-4895-b86f-f718c4a81544',
                    name: 'Plastic morph Balanced',
                    dateCreate: new Date('1997-01-15T21:54:29.920099'),
                    dateUpdate: new Date('1997-01-28T22:29:44.035771'),
                    done: false
                },
                {
                    id: '7dc7631d-3f1e-f8bb-166c-63b52a05db21',
                    name: 'Ports Rupiah Awesome',
                    dateCreate: new Date('1997-01-13T23:07:22.848384'),
                    dateUpdate: new Date('1997-01-27T16:35:23.823537'),
                    done: false
                },
                {
                    id: '0037c991-eb6d-2819-491c-221a1c9c01a8',
                    name: 'matrix Stravenue Usability',
                    dateCreate: new Date('1997-01-06T11:03:38.286677'),
                    dateUpdate: new Date('1997-01-26T17:58:46.010328'),
                    done: false
                },
                {
                    id: '735f337f-755a-4db6-19ef-a092f0c82c8c',
                    name: 'Trail Handcrafted Granite Chicken Shores',
                    dateCreate: new Date('1997-01-16T00:30:24.257496'),
                    dateUpdate: new Date('1997-01-26T10:41:03.225911'),
                    done: false
                },
                {
                    id: 'ea0ff7e6-805e-f8f9-42b3-ef384fc457aa',
                    name: 'Granite quantify PNG',
                    dateCreate: new Date('1997-01-09T14:55:54.461465'),
                    dateUpdate: new Date('1997-01-02T06:59:16.926645'),
                    done: true
                },
                {
                    id: 'f017654b-ccd9-f1f4-5672-b5699171bcd6',
                    name: 'monitor SQL networks',
                    dateCreate: new Date('1997-01-30T19:33:59.212454'),
                    dateUpdate: new Date('1997-01-01T18:09:25.863829'),
                    done: true
                },
                {
                    id: 'da776514-0e56-c4e5-2543-17c6171956b2',
                    name: 'Markets cross-platform secondary',
                    dateCreate: new Date('1997-01-18T04:26:48.731561'),
                    dateUpdate: new Date('1997-01-01T05:01:22.705185'),
                    done: true
                }];
            // #endregion

            let serviceSpy: any;

            beforeEach(() => {
                serviceSpy = jasmine.createSpyObj('HttpGoalService', ['get']);
                serviceSpy.get = (): Observable<Goal[]> => {
                    const subject = new BehaviorSubject<Goal[]>(testData);
                    return subject.asObservable();
                };
                serviceSpy.getAll = (): Observable<Goal[]> => {
                    const subject = new BehaviorSubject<Goal[]>(testDataAll);
                    return subject.asObservable();
                };

                TestBed.configureTestingModule({
                    declarations: [
                        GoalComponent
                    ],
                    imports: [
                        FontAwesomeModule,
                        MatButtonToggleModule,
                        MatPaginatorModule,
                        MatTableModule,
                        MatButtonModule,
                        SharedModule
                    ],
                    schemas: [NO_ERRORS_SCHEMA],
                });

                TestBed.overrideComponent(GoalComponent, {
                    set: {
                        providers: [
                            { provide: HttpGoalService, useValue: serviceSpy },
                        ]
                    }
                }).compileComponents();
            });

            it('get goal data', () => {

                // Arrange
                const fixture = TestBed.createComponent(GoalComponent);
                const component = fixture.debugElement.componentInstance;

                // Act
                component.ngOnInit();

                // Assert
                expect(component.dataSource).not.toBeNull();
            });

            it('event onValChange goals', () => {

                // Arrange
                const fixture = TestBed.createComponent(GoalComponent);
                const component = fixture.debugElement.componentInstance;
                // await fixture.whenStable();

                // Act
                component.onValChange(tableGoalSorting.goals);

                // Assert
                expect(component.dataSource).not.toBeNull();
                expect(component.dataSource.data).toEqual(testData);

                // Act
                component.onValChange(tableGoalSorting.all);

                // Assert
                expect(component.dataSource).not.toBeNull();
                expect(component.dataSource.data).toEqual(testDataAll);

            });

            it('event onDelete', () => {

                // Arrange
                TestBed.overrideComponent(GoalComponent, {
                    set: {
                        providers: [
                            { provide: HttpGoalService, useValue: serviceSpy },
                            { provide: MatDialog, useClass: MatDialogMock }
                        ]
                    }
                }).compileComponents();

                const fixture = TestBed.createComponent(GoalComponent);
                const component = fixture.debugElement.componentInstance;

                spyOn(component.dialog, 'open').and.callThrough();
                spyOn(component, 'delete');

                // Act
                component.onDelete('c55940df-c000-c450-1601-5e9aa3ed8bbc');

                // Assert
                expect(component.dialog.open).toHaveBeenCalled();
                expect(component.delete).toHaveBeenCalled();
            });

            it('event onCheck true', () => {

                // Arrange
                TestBed.overrideComponent(GoalComponent, {
                    set: {
                        providers: [
                            { provide: HttpGoalService, useValue: serviceSpy },
                            { provide: MatDialog, useClass: MatDialogMock }
                        ]
                    }
                }).compileComponents();

                const fixture = TestBed.createComponent(GoalComponent);
                const component = fixture.debugElement.componentInstance;

                spyOn(component.dialog, 'open').and.callThrough();
                spyOn(component, 'check');

                // Act
                component.onCheck('c55940df-c000-c450-1601-5e9aa3ed8bbc', true);

                // Assert
                expect(component.dialog.open).toHaveBeenCalled();
                expect(component.check).toHaveBeenCalled();
            });

            it('event onCheck false', () => {

                // Arrange
                TestBed.overrideComponent(GoalComponent, {
                    set: {
                        providers: [
                            { provide: HttpGoalService, useValue: serviceSpy },
                            { provide: MatDialog, useClass: MatDialogMock }
                        ]
                    }
                }).compileComponents();

                const fixture = TestBed.createComponent(GoalComponent);
                const component = fixture.debugElement.componentInstance;

                spyOn(component.dialog, 'open').and.callThrough();
                spyOn(component, 'check');

                // Act
                component.onCheck('c55940df-c000-c450-1601-5e9aa3ed8bbc', false);

                // Assert
                expect(component.dialog.open).not.toHaveBeenCalled();
                expect(component.check).toHaveBeenCalled();
            });

            it('event onCreate', () => {

                // Arrange
                TestBed.overrideComponent(GoalComponent, {
                    set: {
                        providers: [
                            { provide: HttpGoalService, useValue: serviceSpy },
                            { provide: MatDialog, useClass: MatDialogCreateMock }
                        ]
                    }
                }).compileComponents();

                const fixture = TestBed.createComponent(GoalComponent);
                const component = fixture.debugElement.componentInstance;

                spyOn(component.dialog, 'open').and.callThrough();
                spyOn(component, 'create');

                // Act
                component.onCreate();

                // Assert
                expect(component.dialog.open).toHaveBeenCalled();
                expect(component.create).toHaveBeenCalled();
            });

            it('event onUpdate', () => {

                // Arrange
                TestBed.overrideComponent(GoalComponent, {
                    set: {
                        providers: [
                            { provide: HttpGoalService, useValue: serviceSpy },
                            { provide: MatDialog, useClass: MatDialogCreateMock }
                        ]
                    }
                }).compileComponents();

                const fixture = TestBed.createComponent(GoalComponent);
                const component = fixture.debugElement.componentInstance;

                spyOn(component.dialog, 'open').and.callThrough();
                spyOn(component, 'update');
                spyOnProperty(component, 'dataSource').and.returnValue(new MatTableDataSource<Goal>(testData));

                // Act
                component.onUpdate('c55940df-c000-c450-1601-5e9aa3ed8bbc', true);

                // Assert
                expect(component.dialog.open).toHaveBeenCalled();
                expect(component.update).toHaveBeenCalledWith(jasmine.any(String), jasmine.any(String), jasmine.any(Boolean));
            });

        });
    }
}
