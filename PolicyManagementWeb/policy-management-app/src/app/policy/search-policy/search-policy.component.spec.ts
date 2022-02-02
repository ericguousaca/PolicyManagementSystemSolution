import { PolicyDetailsComponent } from './../policy-details/policy-details.component';
import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchPolicyComponent } from './search-policy.component';
import { PolicyDataService } from 'src/app/services/policy-data.service';
import { FormBuilder } from '@angular/forms';

describe('SearchPolicyComponent', () => {
  let component: SearchPolicyComponent;
  let fixture: ComponentFixture<SearchPolicyComponent>;

  beforeEach(async () => {
    const dataServiceSpy = jasmine.createSpyObj('PolicyDataService', [
      'getAllPolicy',
    ]);
    await TestBed.configureTestingModule({
      declarations: [SearchPolicyComponent, PolicyDetailsComponent],
      providers: [
        FormBuilder,
        { provide: PolicyDataService, useValue: dataServiceSpy },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchPolicyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
