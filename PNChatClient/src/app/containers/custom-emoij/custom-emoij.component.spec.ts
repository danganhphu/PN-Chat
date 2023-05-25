import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomEmoijComponent } from './custom-emoij.component';

describe('CustomEmoijComponent', () => {
  let component: CustomEmoijComponent;
  let fixture: ComponentFixture<CustomEmoijComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomEmoijComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomEmoijComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
