import { POLICYDETAILS } from './../../../../test-data/policy-data';
import { PolicyDetailsComponent } from './../policy-details/policy-details.component';
import { ComponentFixture, fakeAsync, TestBed } from '@angular/core/testing';
import { PolicyDataService } from 'src/app/services/policy-data.service';

import { ListAllPolicyComponent } from './list-all-policy.component';
import { of } from 'rxjs';
import { DebugElement } from '@angular/core';
import { By } from '@angular/platform-browser';

describe('ListAllPolicyComponent', () => {
  let component: ListAllPolicyComponent;
  let fixture: ComponentFixture<ListAllPolicyComponent>;
  let dataService: any;
  let el: DebugElement;

  beforeEach(async () => {
    const dataServiceSpy = jasmine.createSpyObj('PolicyDataService', [
      'getAllPolicy',
    ]);
    await TestBed.configureTestingModule({
      declarations: [ListAllPolicyComponent, PolicyDetailsComponent],
      providers: [{ provide: PolicyDataService, useValue: dataServiceSpy }],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ListAllPolicyComponent);
    component = fixture.componentInstance;
    el = fixture.debugElement;
    dataService = TestBed.inject(PolicyDataService);
    //fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load all 4 test poolicy dateils items in table', fakeAsync(() => {

    const tableBefore = el.queryAll(By.css('app-policy-details table'));
    expect(tableBefore.length).toEqual(0, 'Should not show polict dateil table');

    dataService.getAllPolicy.and.returnValue(of(POLICYDETAILS));
    fixture.detectChanges();

    const tableAfter = el.queryAll(By.css('app-policy-details table'));
    expect(tableAfter.length).toEqual(1, 'Should show polict dateil table');
  }));
});
