import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { User } from 'src/app/core/models/user';
import { UserService } from 'src/app/core/service/user.service';

@Component({
  selector: 'app-list-contact',
  templateUrl: './list-contact.component.html',
  styleUrls: ['./list-contact.component.css']
})

export class ListContactComponent implements OnInit {
  @Output() onClick = new EventEmitter<User>();

  contacts: User[] = [];
  itemIndexSelected: number = -1;

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.getContact();
  }

  getContact() {
    this.userService.getContact()
      .subscribe({
        next: (response: any) => {
          this.contacts = JSON.parse(response["data"]);
          this.itemIndexSelected = -1;
        },
        error: (error) => console.log('error: ', error),
      });
  }

  openContact(indexContact: any) {
    this.itemIndexSelected = indexContact;
    this.onClick.emit(this.contacts[indexContact]);
  }
}
