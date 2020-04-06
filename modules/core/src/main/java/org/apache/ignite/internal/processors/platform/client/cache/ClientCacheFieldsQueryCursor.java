/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package org.apache.ignite.internal.processors.platform.client.cache;

import org.apache.ignite.cache.query.FieldsQueryCursor;
import org.apache.ignite.internal.binary.BinaryRawWriterEx;
import org.apache.ignite.internal.processors.cache.query.QueryCursorEx;
import org.apache.ignite.internal.processors.platform.client.ClientConnectionContext;
import org.apache.ignite.internal.processors.query.GridQueryFieldMetadata;

import java.util.List;

/**
 * Query cursor holder.
  */
class ClientCacheFieldsQueryCursor extends ClientCacheQueryCursor<List> {
    /** Column count. */
    private final int columnCount;

    /** Cursor. */
    private FieldsQueryCursor<List> cursor;

    /**
     * Ctor.
     *
     * @param cursor   Cursor.
     * @param pageSize Page size.
     * @param ctx      Context.
     */
    ClientCacheFieldsQueryCursor(FieldsQueryCursor<List> cursor, int pageSize, ClientConnectionContext ctx) {
        super(cursor, pageSize, ctx);

        columnCount = cursor.getColumnsCount();
        this.cursor = cursor;
    }

    /** {@inheritDoc} */
    @Override void writeEntry(BinaryRawWriterEx writer, List e) {
        assert e.size() == columnCount;

        for (Object o : e)
            writer.writeObjectDetached(o);
    }

    /**
     * @return Query metadata.
     */
    public List<GridQueryFieldMetadata> fieldsMeta() {
        assert cursor instanceof QueryCursorEx;

        QueryCursorEx cursorExtended = (QueryCursorEx) cursor;

        return cursorExtended.fieldsMeta();
    }
}
