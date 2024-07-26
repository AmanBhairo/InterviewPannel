import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-reportdecider',
  templateUrl: './reportdecider.component.html',
  styleUrls: ['./reportdecider.component.css']
})
export class ReportdeciderComponent implements OnInit{
  constructor(private route:ActivatedRoute) {}
  controllerDecider:string ='detail'
  ngOnInit(): void {
    this.route.params.subscribe((params)=>{
      this.controllerDecider = params['reportDeciderVal'];
     }); 
    
     this.selectedComponent =1;
  }
  selectedComponent:number =1;
  showComponent(num:number){
    this.selectedComponent = num;
  }
}
