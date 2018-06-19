import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'groupby'
})
export class GroupbyPipe implements PipeTransform {

  transform(value: any, property?: string): any {
    if (value) {
      const groupedValue = value.reduce((previous, current) => {
        if (!previous[current[property]]) {
          previous[current[property]] = [current];
        } else {
          previous[current[property]].push(current);
        }
        return previous;
      }, {});

      // this will return an array of objects, each object containing a group of objects
      return Object.keys(groupedValue).map(key => ({ key, value: groupedValue[key] }));
    }
    return null;
  }
}
