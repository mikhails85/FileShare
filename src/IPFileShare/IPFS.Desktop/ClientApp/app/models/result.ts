import {VoidResult} from './void-result';

export class Result<T> extends VoidResult
{
    public value : T;
}