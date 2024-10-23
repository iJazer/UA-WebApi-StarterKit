export class HandleFactory {
   private static counter: number = 0;
   public static increment(): number {
      return ++this.counter;
   }
}
