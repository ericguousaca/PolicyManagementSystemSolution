import { POLICYDETAILS } from './../../../../test-data/policy-data';
import { PolicyDetail } from 'src/app/models/policy-detail.model';
import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PolicyDetailsComponent } from './policy-details.component';
import { DebugElement } from '@angular/core';
import { By } from '@angular/platform-browser';
import { GridModule } from '@progress/kendo-angular-grid';

describe('PolicyDetailsComponent', () => {
  let component: PolicyDetailsComponent;
  let fixture: ComponentFixture<PolicyDetailsComponent>;
  let el: DebugElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PolicyDetailsComponent],
      imports: [GridModule],
    })
      .compileComponents()
      .then(() => {
        console.log('Done async await');
      });
  });

  beforeEach(() => {
    console.log('Start create component');
    fixture = TestBed.createComponent(PolicyDetailsComponent);
    component = fixture.componentInstance;
    el = fixture.debugElement;
    fixture.detectChanges();
    console.log('Done creating componenet');
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('shoud display 5 test policy detail items in table', () => {
    const tableBefore = el.queryAll(By.css('.k-grid-table'));

    console.log(tableBefore);

    expect(tableBefore.length).toEqual(
      0,
      'Should not display table befroe data binding'
    );

    component.policyDetails = POLICYDETAILS;
    component.gridViewData = {
      data: POLICYDETAILS,
      total: POLICYDETAILS.length,
    };

    fixture.detectChanges();

    const tableAfter = el.queryAll(By.css('table.k-grid-table'));

    console.log('Table After');
    console.log(tableAfter);
    expect(tableAfter.length).toEqual(
      1,
      'Could not find the policy details table'
    );

    const rows = el.queryAll(By.css('table.k-grid-table tbody tr'));

    expect(rows.length).toEqual(4, 'Total of test polict details must be 4');
  });
});
