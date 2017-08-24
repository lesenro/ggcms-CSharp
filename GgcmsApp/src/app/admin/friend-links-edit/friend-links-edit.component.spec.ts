import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FriendLinksEditComponent } from './friend-links-edit.component';

describe('FriendLinksEditComponent', () => {
  let component: FriendLinksEditComponent;
  let fixture: ComponentFixture<FriendLinksEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FriendLinksEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FriendLinksEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
