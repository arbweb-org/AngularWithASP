import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {
  PageChangeEvent,
  GridDataResult,
} from "@progress/kendo-angular-grid";

interface User {
  Id: number;
  FirstName: string;
  LastName: string;
  Email: string;
}

interface UserDTO {
  Users: User[];
  Skip: number;
  Total: number;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public users: UserDTO = {
    Users: [],
    Skip: 0,
    Total: 0
  };

  public busy: boolean = true;                                            // Disables interactivity while communicating
  public editing: boolean = false;                                        // Should form be visible
  public newUser: boolean = false;                                        // Adding new user or updating existing
  public user: User = { Id: 0, FirstName: "", LastName: "", Email: "" }   // User to Add or update

  // Grid pagination properties
  public gridView: GridDataResult =
    {
      data: [],
      total: 0
    };
  public info = true;
  public previousNext = true;
  public pageSize = 3;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.loadItems();;
  }

  public pageChange({ skip, take }: PageChangeEvent): void {
    this.users.Skip = skip;
    this.loadItems();
  }

  public loadItems(): void {
    this.busy = true;
    this.editing = false;

    let params = new HttpParams()
      .append("skip", this.users.Skip)
      .append("take", this.pageSize);

    this.http.get<UserDTO>('/api/users/get', { params: params }).subscribe(
      (result) => {
        this.users = result;
        this.busy = false;

        this.gridView = {
          data: this.users.Users,
          total: this.users.Total
        };
      },
      (error) => {
        alert('Could not load users! Try to refresh the page.');
      }
    );
  }

  Delete(id: number) {
    if (this.busy) { return; }

    this.busy = true;
    this.editing = false;

    let params = new HttpParams()
      .append("id", id);

    this.http.get<UserDTO>('/api/users/delete', { params: params }).subscribe(
      (result) => {
        this.loadItems();
      },
      (error) => {
        alert('Could not update users list.');
        this.loadItems();
      }
    );
  }

  Add() {
    if (this.busy) { return; }

    this.editing = true;
    this.newUser = true;
    this.user = { Id: 0, FirstName: "", LastName: "", Email: "" }
  }

  Modify(id: number) {
    if (this.busy) { return; }

    this.editing = true;
    this.newUser = false;
    this.user = this.users.Users.filter(x => x.Id == id)[0];
  }

  Save() {
    if (this.busy) { return; }
    this.busy = true;

    let params = new HttpParams()
      .append("user", JSON.stringify(this.user));

    let action = this.newUser ? "add" : "update";
    this.http.get<UserDTO>('/api/users/' + action, { params: params }).subscribe(
      (result) => {
        this.loadItems();
      },
      (error) => {
        alert('Could not update users list.');
        this.loadItems();
      }
    );
  }

  title = 'Stock Market';
}
