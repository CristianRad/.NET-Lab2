import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public tasks: Task[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Task[]>(baseUrl + 'api/tasks').subscribe(result => {
      this.tasks = result;
    }, error => console.error(error));
  }
}

interface Task {
  id: number,
  title: string,
  description: string,
  added: Date,
  deadline: Date,
  importance: Importance,
  state: State,
  closedAt: Date,
  comments: Comment[],
  numberOfComments: number
}

enum State {
  Open = 1,
  InProgress = 2,
  Closed = 3
}

enum Importance {
  Low = 1,
  Medium = 2,
  High = 3
}
