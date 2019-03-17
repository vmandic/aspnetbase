import anotherFn from "./anotherFile";

const helloWorld = function (): string {
  anotherFn();
  return "Hello World from TypeScript!";
};

export class cl1 {
  aProp1: string | null = null;
}

export class cl2 extends cl1 {
  aProp2: string | null = null;
}

export default helloWorld;
