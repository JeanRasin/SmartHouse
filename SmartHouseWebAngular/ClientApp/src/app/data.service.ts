export class DataService {
  private data: string[] = ["Apple iPhone XR", "Samsung Galaxy S9", "Nokia 9"];

  getData(): string[] {
    return this.data;
  }

  addData(name: string) {
    this.data.push(name);
  }

}
