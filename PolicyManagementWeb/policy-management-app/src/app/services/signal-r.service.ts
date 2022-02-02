import { PolicyDetail } from './../models/policy-detail.model';
import { Injectable } from '@angular/core';
import * as SignalR from '@microsoft/signalr';
import { AppConstants } from '../models/app-constants.model';
import { SearchPolicyParams } from '../models/search-policy-params.model';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private baseUrl: string = 'https://localhost:44321';
  public HubConnection!: SignalR.HubConnection;
  public ConnectionId!: string;

  constructor() {}

  public startConnection() {
    this.HubConnection = new SignalR.HubConnectionBuilder()
      .withUrl(`${this.baseUrl}/PolicyHub`)
      .build();

    this.HubConnection.start()
      .then(() => console.log('Hub Connection started'))
      .then(() => this.getConnectionId())
      .catch((err) => console.log(`Hub Connection Error:${err}`));
  }

  public getConnectionId = () => {
    this.HubConnection.invoke('getconnectionid').then((data) => {
      console.log('ConnectionId: ' + data);
      this.ConnectionId = data;
    });
  };
}
