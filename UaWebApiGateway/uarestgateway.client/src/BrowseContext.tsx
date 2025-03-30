import * as React from 'react';
import { IBrowseContext } from './BrowseProvider';
import { IBrowsedNode } from './service/IBrowsedNode';


export const BrowseContext = React.createContext<IBrowseContext>({
   stateChangeCount: 0,
   nodes: new Map<string, IBrowsedNode>,
   visibleNodes: [],
   setVisibleNodes: () => { },
   lastCompletedRequest: undefined,
   browseChildren: (): Promise<void> => Promise.resolve(),
   readValues: (): Promise<void> => Promise.resolve()
});
