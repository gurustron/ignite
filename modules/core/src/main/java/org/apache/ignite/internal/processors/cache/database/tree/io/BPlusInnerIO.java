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

package org.apache.ignite.internal.processors.cache.database.tree.io;

import java.nio.ByteBuffer;

/**
 * Abstract IO routines for B+Tree inner pages.
 */
public abstract class BPlusInnerIO<L> extends BPlusIO<L> {
    /** */
    protected static final int SHIFT_LEFT = ITEMS_OFF;

    /** */
    protected static final int SHIFT_LINK = SHIFT_LEFT + 8;

    /** */
    protected final int SHIFT_RIGHT = SHIFT_LINK + itemSize;

    /**
     * @param ver Page format version.
     * @param canGetRow If we can get full row from this page.
     * @param itemSize Single item size on page.
     */
    protected BPlusInnerIO(int ver, boolean canGetRow, int itemSize) {
        super(ver, false, canGetRow, itemSize);
    }

    /** {@inheritDoc} */
    @Override public final int getMaxCount(ByteBuffer buf) {
        // The structure of the page is the following:
        // |ITEMS_OFF|w|A|x|B|y|C|z|
        // where capital letters are data items, lowercase letters are 8 byte page references.
        return (buf.capacity() - ITEMS_OFF - 8) / (itemSize + 8);
    }

    /**
     * @param buf Buffer.
     * @param idx Index.
     * @return Page ID.
     */
    public final long getLeft(ByteBuffer buf, int idx) {
        return buf.getLong(offset(idx, SHIFT_LEFT));
    }

    /**
     * @param buf Buffer.
     * @param idx Index.
     * @param pageId Page ID.
     */
    public final void setLeft(ByteBuffer buf, int idx, long pageId) {
        buf.putLong(offset(idx, SHIFT_LEFT), pageId);

        assert pageId == getLeft(buf, idx);
    }

    /**
     * @param buf Buffer.
     * @param idx Index.
     * @return Page ID.
     */
    public final long getRight(ByteBuffer buf, int idx) {
        return buf.getLong(offset(idx, SHIFT_RIGHT));
    }

    /**
     * @param buf Buffer.
     * @param idx Index.
     * @param pageId Page ID.
     */
    public final void setRight(ByteBuffer buf, int idx, long pageId) {
        buf.putLong(offset(idx, SHIFT_RIGHT), pageId);

        assert pageId == getRight(buf, idx);
    }

    /** {@inheritDoc} */
    @Override public final void copyItems(ByteBuffer src, ByteBuffer dst, int srcIdx, int dstIdx, int cnt,
        boolean cpLeft) {
        assert srcIdx != dstIdx || src != dst;

        if (dstIdx > srcIdx) {
            for (int i = cnt - 1; i >= 0; i--) {
                dst.putLong(offset(dstIdx + i, SHIFT_RIGHT), src.getLong(offset(srcIdx + i, SHIFT_RIGHT)));
                dst.putLong(offset(dstIdx + i, SHIFT_LINK), src.getLong(offset(srcIdx + i, SHIFT_LINK)));
            }

            if (cpLeft)
                dst.putLong(offset(dstIdx, SHIFT_LEFT), src.getLong(offset(srcIdx, SHIFT_LEFT)));
        }
        else {
            if (cpLeft)
                dst.putLong(offset(dstIdx, SHIFT_LEFT), src.getLong(offset(srcIdx, SHIFT_LEFT)));

            for (int i = 0; i < cnt; i++) {
                dst.putLong(offset(dstIdx + i, SHIFT_RIGHT), src.getLong(offset(srcIdx + i, SHIFT_RIGHT)));
                dst.putLong(offset(dstIdx + i, SHIFT_LINK), src.getLong(offset(srcIdx + i, SHIFT_LINK)));
            }
        }
    }

    /**
     * @param idx Index of element.
     * @param shift It can be either link itself or left or right page ID.
     * @return Offset from byte buffer begin in bytes.
     */
    protected final int offset(int idx, int shift) {
        return shift + (8 + itemSize) * idx;
    }
}
