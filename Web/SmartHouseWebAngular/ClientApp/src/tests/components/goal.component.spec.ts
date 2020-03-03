import { NO_ERRORS_SCHEMA } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { Goal, SharedModule, HttpGoalService } from 'src/app/shared';
import { Observable, BehaviorSubject } from 'rxjs';
import { MatTableModule, MatPaginatorModule, MatButtonToggleModule, MatButtonModule } from '@angular/material';
import { GoalComponent, tableSorting } from 'src/app/goal/goal.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

//todo: сделать проверку на то если нет связи с апи.
describe('Goal component', () => {

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

    beforeEach(() => {
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
    });

    it('get goal data', () => {
        let serviceSpy = jasmine.createSpyObj('HttpGoalService', ['get']);
        serviceSpy.get = (): Observable<Goal[]> => {
            const subject = new BehaviorSubject<Goal[]>(testData);
            return subject.asObservable();
        };

        TestBed.overrideComponent(GoalComponent, {
            set: {
                providers: [
                    { provide: HttpGoalService, useValue: serviceSpy }
                ]
            }
        }).compileComponents();

        const fixture = TestBed.createComponent(GoalComponent);
        const goalComponent = fixture.debugElement.componentInstance;
        goalComponent.ngOnInit();

        expect(goalComponent.dataSource).not.toBeNull();
    });

    it('event onValChange goals', () => {
        let serviceSpy = jasmine.createSpyObj('HttpGoalService', ['get']);
        serviceSpy.get = (): Observable<Goal[]> => {
            const subject = new BehaviorSubject<Goal[]>(testData);
            return subject.asObservable();
        };
        serviceSpy.getAll = (): Observable<Goal[]> => {
            const subject = new BehaviorSubject<Goal[]>(testDataAll);
            return subject.asObservable();
        };

        TestBed.overrideComponent(GoalComponent, {
            set: {
                providers: [
                    { provide: HttpGoalService, useValue: serviceSpy }
                ]
            }
        }).compileComponents();

        const fixture = TestBed.createComponent(GoalComponent);
        const goalComponent = fixture.debugElement.componentInstance;

        goalComponent.onValChange(tableSorting.goals);

        expect(goalComponent.dataSource).not.toBeNull();
        expect(goalComponent.dataSource.data).toEqual(testData);

        goalComponent.onValChange(tableSorting.all);

        expect(goalComponent.dataSource).not.toBeNull();
        expect(goalComponent.dataSource.data).toEqual(testDataAll);

    });

    it('event onDelete', () => {  



    });

});