import { Component, OnInit, } from '@angular/core';
import { BaseComponent } from '../../base.component';
import { LocalStorageService } from 'angular-2-local-storage';
import { Router } from '@angular/router';
import { Interceptor } from 'ng2-interceptors';
import { PubSubService } from '../../interceptor/pub-service';

@Component({
  selector: 'sp-dashboard',
  templateUrl: './dashboard-view.component.html',
})

export class DashboardViewComponent extends BaseComponent implements OnInit, Interceptor {

  constructor(
    localStorageService: LocalStorageService,
    router: Router,
    public pubsub: PubSubService
  ) {
    super(localStorageService, router);
  }

  ngOnInit() {
    //this.pubsub.beforeRequest.subscribe(data => this.showLoader = true);
    //this.pubsub.afterRequest.subscribe(data => this.showLoader = false);
  }

}
